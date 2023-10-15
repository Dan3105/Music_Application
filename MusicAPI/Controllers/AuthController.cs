using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Data.Entities;
using MusicAPI.Models;
using MusicAPI.Repository;
using MusicAPI.Repository.Interface;
using MusicAPI.Services;
using NuGet.Packaging.Signing;
using System.IdentityModel.Tokens.Jwt;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IJwtService _jwtService;
        private readonly MusicAPIContext _context;
        private readonly IUserRolesRepository _userRolesRepository;

        public AuthController(IJwtService jwtService, MusicAPIContext context, IUserRolesRepository userRolesRepository)
        {
            _jwtService = jwtService;
            _context = context;
            _userRolesRepository = userRolesRepository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> PostUser(RegisterModel registerForm)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'MusicAPIContext.User'  is null.");
            }

            // Validate the user input.
            if (!ModelState.IsValid)
            {
                // Return a BadRequestObjectResult with the validation errors.
                return BadRequest(ModelState);
            }

            // Check if the user already exists in the database.
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == registerForm.Email);
            if (existingUser != null)
            {
                // The user already exists. Return a BadRequestObjectResult with an error message.
                return BadRequest("The user already exists.");
            }

            User new_user = new User();
            new_user.Email = registerForm.Email;
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(registerForm.Password);
            new_user.password = hashPassword;
            try
            {
                _context.User.Add(new_user);
            }
            catch (Exception e)
            {
                if (Utlity.Utility.IsInDebugMode())
                    return BadRequest(e.Message);
                return BadRequest("Error");
            }
            await _context.SaveChangesAsync();

            return Ok("Successfully create user!");
        }

        //POST: login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(RegisterModel request)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'MusicAPIContext.User'  is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(request.Password, user.password))
                {
                    Response.Cookies.Append("user", user.Public_id.ToString());
                    return Ok(user);
                }
            }

            return BadRequest("Incorrect password or email hasn't been registed");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("user");
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AuthToken([FromBody]AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Username and password must be provided" });
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.password))
                {
                    return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Username or password not correct" });
                }
                else if (user.Is_activate == false)
                {
                    return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Username has been blocked from this website" });
                }
            }

            var tokenForm = GenerateFormToken(user);

            var authResponse = await _jwtService.GetTokenAsync(tokenForm);
            if (authResponse == null)
                return Unauthorized();

            return Ok(authResponse);
        }

        private TokenFormRequest GenerateFormToken(User? user)
        {
            var roleUser = _userRolesRepository.GetAllRolesNameFromUser(user.Id);

            TokenFormRequest tokenForm = new TokenFormRequest
            {
                IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                UserEmail = user.Email,
                UserId = user.Id,
                Roles = roleUser.ToArray()
            };

            return tokenForm;
        }

        [HttpPost("[action]")]

        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Token must be provided" });
            }
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();   
            var token = GetJwtToken(request.ExpiredToken);
            var userRefreshToken = _context.UserRefreshToken.FirstOrDefault(x =>
            x.IsInvalidated == false && x.Token == request.ExpiredToken && x.RefreshToken == request.RefreshToken
            && x.IpAddress == ipAddress);

            AuthResponse response = ValidateDetails(userRefreshToken, token);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            var tokenFormRequest = GenerateFormToken(userRefreshToken.User);

            userRefreshToken.IsInvalidated = true;
            _context.UserRefreshToken.Update(userRefreshToken);
            await _context.SaveChangesAsync();
            
            //var userEmail = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
            var newResponse = await _jwtService.GetRefreshTokenAsync(tokenFormRequest);
        
             return Ok(newResponse);
        }   

        private AuthResponse ValidateDetails(UserRefreshToken? userRefreshToken, JwtSecurityToken token)
        {
            if (userRefreshToken == null || token == null)
                return new AuthResponse { IsSuccess = false, Reason = "Invalid token" };
            if (token.ValidTo > DateTime.UtcNow)
                return new AuthResponse { IsSuccess = false, Reason = "Token has not been expired" };
            if(!userRefreshToken.IsActive)
                return new AuthResponse { IsSuccess = false, Reason = "Refresh Token has expired" };
            return new AuthResponse { IsSuccess = true };
        }
        

        private JwtSecurityToken GetJwtToken(string expiredToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(expiredToken);
        }
    }
}
