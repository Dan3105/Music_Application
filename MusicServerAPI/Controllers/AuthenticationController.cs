using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;
using MusicServerAPI.Model.ModelAuthentication;
using MusicServerAPI.Repository;
using MusicServerAPI.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MusicServerAPI.Controllers
{
    [Route("/api/Auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IJwtService _jwtService;
        private readonly MusicServerAPIContext _context;
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IJwtService IJwtService, MusicServerAPIContext MusicServerAPIContext, IUserRepository IUserRepository)
        {
            _jwtService = IJwtService;
            _context = MusicServerAPIContext;
            _userRepository = IUserRepository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> PostUser(RegisterDTO registerForm)
        {
            if (_context.Users == null)
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
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerForm.Email);
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
        public async Task<IActionResult> AuthToken([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Username and password must be provided" });
            }

            var user = _userRepository.GetUserByLogin(request.Email, request.Password);
            if (user != null)
            {
                if (user.Is_activate == false)
                    return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Username has been blocked from this website" });
            }
            else
            {
                return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Username or password not correct" });

            }

            var tokenForm = GenerateUserRequest(user);

            var authResponse = await _jwtService.GetTokenAsync(tokenForm);
            if (authResponse == null)
                return Unauthorized();
            SetAccessToken(authResponse);
            SetRefreshToken(authResponse);

            return Ok(tokenForm);
        }

        private UserRequest GenerateUserRequest(User user)
        {
            var roles = user.UserRoles
                ?.Where(u => u.UserId == user.Id)
                ?.Select(rt => rt.Role)
                ?.Select(r => r.RoleName)
                ?.ToArray();
            UserRequest userRequest = new UserRequest
            {
                UserEmail = user.Email,
                Roles = roles
            };
            return userRequest;

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Token must be provided" });
            }

            string expiredToken = Request.Cookies["access_token"];
            string refreshToken = Request.Cookies["refresh_token"];

            if (refreshToken == null )
            {
                return BadRequest(new AuthResponse() { IsSuccess = false, Reason = "Refresh token not found" });
            }
            JwtSecurityToken token = null;
            if (expiredToken != null)
                token = GetJwtToken(expiredToken);

            var user = _context.Users.FirstOrDefault(x => x.RefreshToken == refreshToken);

            AuthResponse response = ValidateDetails(user, token);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            var tokenFormRequest = GenerateUserRequest(user);

            //var userEmail = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
            var newResponse = await _jwtService.GetRefreshTokenAsync(tokenFormRequest);
            SetAccessToken(newResponse);
            SetRefreshToken(newResponse);
            return Ok(newResponse);
        }

        private AuthResponse ValidateDetails(User? user, JwtSecurityToken token)
        {
            if (user == null)
                return new AuthResponse { IsSuccess = false, Reason = "Invalid token" };
            if (token != null && token.ValidTo > DateTime.UtcNow)
                return new AuthResponse { IsSuccess = false, Reason = "Token has not been expired" };
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
                                Expires = authResponse.RefreshToken.Expired.AddSeconds(5),
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
    }
}
