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
        private readonly IUserRepository _userRepository;

        public AuthController(IJwtService jwtService, MusicAPIContext context, IUserRolesRepository userRolesRepository, IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _context = context;
            _userRolesRepository = userRolesRepository;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> PostUser(RegisterDTO registerForm)
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
                return BadRequest(new { message = "The user already exists." });
            }

            await _userRepository.Create(registerForm);

            return Ok("Successfully create user!");
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");
            HttpContext.Response.Cookies.Delete("refresh_token");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> AuthToken([FromBody] AuthRequest request)
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
            SetAccessToken(authResponse);
            SetRefreshToken(authResponse);

            return Ok(tokenForm);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Token must be provided" });
            }
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            string expiredToken = Request.Cookies["access_token"];
            var token = GetJwtToken(expiredToken);
            string refreshToken = Request.Cookies["refresh_token"];

            if (refreshToken == null)
            {
                return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Refresh token not found" });
            }

            var userRefreshToken = _context.UserRefreshToken.FirstOrDefault(x =>
            x.IsInvalidated == false && x.Token == expiredToken && x.RefreshToken == refreshToken
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
            if (!userRefreshToken.IsActive)
                return new AuthResponse { IsSuccess = false, Reason = "Refresh Token has expired" };
            return new AuthResponse { IsSuccess = true };
        }


        private JwtSecurityToken GetJwtToken(string expiredToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(expiredToken);
        }


        private void SetRefreshToken(AuthResponse authResponse)
        {
            HttpContext.Response.Cookies.Append("refresh_token", authResponse.RefreshToken.Token,
                            new CookieOptions
                            {
                                Expires = authResponse.RefreshToken.Expired,
                                HttpOnly = true,
                                Secure = true,
                                IsEssential = true,
                                SameSite = SameSiteMode.None
                            });
        }

        private void SetAccessToken(AuthResponse authResponse)
        {
            HttpContext.Response.Cookies.Append("access_token", authResponse.AccessToken.Token,
                            new CookieOptions
                            {
                                Expires = authResponse.AccessToken.Expired,
                                HttpOnly = true,
                                Secure = true,
                                IsEssential = true,
                                SameSite = SameSiteMode.None
                            });
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
    }
}
