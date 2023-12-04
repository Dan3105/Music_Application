using Microsoft.IdentityModel.Tokens;
using MusicServerAPI.Data;
using MusicServerAPI.Model.ModelAuthentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MusicServerAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly MusicServerAPIContext _context;
        private readonly IConfiguration _configuration;

        public JwtService(MusicServerAPIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponse> GetRefreshTokenAsync(UserRequest request)
        {
            var refreshToken = GenerateRefreshToken();
            var accessToken = GenerateAccessToken(request.UserEmail, request.Roles);
            AuthResponse newAuthResponse = await UpdateToken(request.UserEmail, accessToken, refreshToken);
            newAuthResponse.UserRequest = request;
            return newAuthResponse;
        }

        public async Task<AuthResponse> GetTokenAsync(UserRequest request)
        {
            var user = _context.Users?.FirstOrDefault(u => u.Email == request.UserEmail);
            if (user == null)
            {
                return await Task.FromResult(new AuthResponse { IsSuccess = false, Reason = "Invalid Request" });
            }

            AccessToken accessToken = GenerateAccessToken(user.Email, user.UserRoles?.Select(role => role.Role.RoleName).ToArray());
            RefreshToken refreshToken = GenerateRefreshToken();
            
            var authResponse = await UpdateToken(user.Email, accessToken, refreshToken);
            authResponse.UserRequest = request;
            return authResponse;
        }

        private async Task<AuthResponse> UpdateToken(string email,AccessToken accessToken, RefreshToken refreshToken)
        {
            var user = _context.Users?.FirstOrDefault(u => u.Email == email);
            if(user == null)
            {
                return new AuthResponse { IsSuccess = false, Reason = "Invalid Request" };
            }
            user.RefreshToken = refreshToken.Token;
            user.CreatedTokenDate = refreshToken.Created;
            user.ExpirationTokenDate = refreshToken.Expired;
            _context.Users.Update(user);
            _context.SaveChanges();

            return new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public Task<bool> IsValidated(string Token, string ipAddress)
        {
            throw new NotImplementedException();
        }

        private RefreshToken GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            using (var cryptoProvider = RandomNumberGenerator.Create())
            {
                cryptoProvider.GetBytes(byteArray);
            }
            var CreatedDate = DateTime.UtcNow;
            var ExpiredDate = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSetting:RefreshLifeTimeMinutes"));
            return new RefreshToken
            {
                Token = Convert.ToBase64String(byteArray),
                Created = CreatedDate,
                Expired = ExpiredDate,
            };
        }

        private AccessToken GenerateAccessToken(string email, string[] roles)
        {
            var created = DateTime.UtcNow;
            var expired = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSetting:AccessLifeTimeMinutes"));
            
            var jwtKey = _configuration.GetValue<string>("JwtSetting:Key");
            var keyByte = Encoding.ASCII.GetBytes(jwtKey);

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, email)
            }) ;

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    Claim claim = new Claim(ClaimTypes.Role, role);
                    identity.AddClaim(claim);
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = expired,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyByte),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            string tokenString = tokenHandler.WriteToken(token);

            return new AccessToken { Created = created, Expired = expired, Token = tokenString };
        }
    
    
    }
}
