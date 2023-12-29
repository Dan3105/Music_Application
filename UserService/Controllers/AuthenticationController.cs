using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entity;
using UserService.Model;
using UserService.Model.ModelAuthentication;
using UserService.Repository;
using UserService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace UserService.Controllers
{
    [Route("/api/UserService/Auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        //private readonly ISongRepository _songRepository;

        public AuthenticationController(IJwtService IJwtService,
            IUserRepository IUserRepository)
        {
            _jwtService = IJwtService;
            _userRepository = IUserRepository;
            //_songRepository = songRepository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> PostUser(RegisterDTO registerForm)
        {
            if (!ModelState.IsValid)
            {
               
                return BadRequest(ModelState);
            }

            var existingUser = await _userRepository.GetUser(registerForm.Email);
            if (existingUser != null)
            {
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
                return BadRequest(new { message = "Username and password must be provided" });
            }

            var user = await _userRepository.GetUserByLogin(request.Email, request.Password);
            if (user != null)
            {
                if (user.Is_activate == false)
                    return BadRequest(new { message = "Username has been blocked from this website" });
                
            }
            else
            {
                return BadRequest(new { message = "Username or password not correct" });
            }

            var tokenForm = GenerateUserRequest(user);
            if(tokenForm==null)
            {
                return BadRequest();
            }
            var authResponse = await _jwtService.GetTokenAsync(tokenForm);
            if (authResponse == null)
                return Unauthorized();
            SetAccessToken(authResponse);
            SetRefreshToken(authResponse);

            return Ok(authResponse);
        }

        private UserRequest GenerateUserRequest(User user)
        {
            try
            {
                var roles = user.UserRoles
  
                    ?.Where(u => u.UserId == user.Id)
                    ?.Select(rt => rt.Role)
                    ?.Select(r => r.RoleName)
                    ?.ToArray();
                UserRequest userRequest = new UserRequest
                {
                    Id = user.Id,
                    UserEmail = user.Email,
                    Roles = user.UserRoles.Select(usr => usr.Role).Select(r => r.RoleName).ToArray(),
                    //Favorites = user?.FavoriteSongs?.Select(p => p.SongId).ToArray()
                };
                return userRequest;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken()
        {

            string expiredToken = HttpContext.Request.Cookies["access_token"];
            string refreshToken = HttpContext.Request.Cookies["refresh_token"];

            if (refreshToken == null )
            {
                return BadRequest("Refresh token not found");
            }
            JwtSecurityToken token = null;
            if (expiredToken != null)
                token = GetJwtToken(expiredToken);

            //var user = _context.Users.FirstOrDefault(x => x.RefreshToken == refreshToken);
            var user = await _userRepository.GetUserByToken(refreshToken);
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
                                //Domain= "localhost",
                                Expires = authResponse.AccessToken.Expired,
                                HttpOnly = true,
                                Secure = true,
                                IsEssential = true,
                                SameSite = SameSiteMode.None
                            });
        }
    
    }
}
