using Microsoft.IdentityModel.Tokens;
using MusicAPI.Data;
using MusicAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace MusicAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly MusicAPIContext _context;
        private readonly IConfiguration _configuration;

        public JwtService(MusicAPIContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<AuthResponse> GetRefreshTokenAsync(TokenFormRequest tokenForm)
        {
            var refreshToken = GenerateRefreshToken();
            var accessToken = GenerateToken(tokenForm.UserEmail, tokenForm.Roles);

            return await SaveTokenDetails(tokenForm.IpAddress, tokenForm.UserId, accessToken, refreshToken);
        }

        public async Task<AuthResponse> GetTokenAsync(TokenFormRequest tokenForm)
        {
            var user = _context.User.FirstOrDefault(u => u.Email.Equals(tokenForm.UserEmail));

            if (user == null)
                return await Task.FromResult<AuthResponse>(null);

            string tokenString = GenerateToken(user.Email, tokenForm.Roles);
            string refreshToken = GenerateRefreshToken();
            return await SaveTokenDetails(tokenForm.IpAddress, user.Id, tokenString, refreshToken);
        }

        private async Task<AuthResponse> SaveTokenDetails(string ipAddress, int userId, string tokenString, string refreshToken)
        {
            var userRefreshToken = new UserRefreshToken()
            {
                CreatedDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSetting:RefreshLifeTimeMinutes")),
                IpAddress = ipAddress,
                IsInvalidated = false,
                RefreshToken = refreshToken,
                Token = tokenString,
                UserId = userId
            };

            await _context.UserRefreshToken.AddAsync(userRefreshToken);
            await _context.SaveChangesAsync();
            return new AuthResponse() { Token = tokenString, IsSuccess = true, RefreshToken = refreshToken };
        }

        private string GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            using (var cryptoProvider = RandomNumberGenerator.Create())
            {
                cryptoProvider.GetBytes(byteArray);
            } 

            return Convert.ToBase64String(byteArray);
        }

        private string GenerateToken(string? userEmail, string[] listRoles)
        {
            var jwtKey = _configuration.GetValue<string>("JwtSetting:Key");
            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, userEmail)
            });

            foreach(var role in listRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            

            var tokenHandler = new JwtSecurityTokenHandler();

            var description = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, userEmail),
                    
                }),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSetting:AccessLifeTimeMinutes")),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(description);
            string tokenString = tokenHandler.WriteToken(token);
            //string tokenString = await Task.FromResult(tokenHandler.WriteToken(token));
            return tokenString;
        }

        public async Task<bool> IsValidated(string Token, string ipAddress)
        {
            var _isValid = _context.UserRefreshToken.FirstOrDefault(x => x.Token == Token && x.IpAddress == ipAddress) != null;
            return await Task.FromResult(_isValid);
        }
    }
}
