using Microsoft.IdentityModel.Tokens;
using MusicAPI.Data;
using MusicAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using MusicAPI.Data.Entities;

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

            AccessToken accessToken = GenerateToken(user.Email, tokenForm.Roles);
            string refreshToken = GenerateRefreshToken();
            return await SaveTokenDetails(tokenForm.IpAddress, user.Id, accessToken, refreshToken);
        }

        private async Task<AuthResponse> SaveTokenDetails(string ipAddress, int userId, AccessToken access, string refreshToken)
        {
            var CreatedDate = DateTime.UtcNow;
            var ExpiredDate = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSetting:RefreshLifeTimeMinutes"));
            var userRefreshToken = new UserRefreshToken()
            {
                CreatedDate = CreatedDate,
                ExpirationDate = ExpiredDate,
                IpAddress = ipAddress,
                IsInvalidated = false,
                RefreshToken = refreshToken,
                Token = access.Token,
                UserId = userId
            };

            await _context.UserRefreshToken.AddAsync(userRefreshToken);
            await _context.SaveChangesAsync();
            return new AuthResponse() { AccessToken = access, RefreshToken = new RefreshToken { Token = refreshToken, Created = CreatedDate, Expired = ExpiredDate } };
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

        private AccessToken GenerateToken(string? userEmail, string[] listRoles)
        {
            var create = DateTime.UtcNow;
            var expired = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSetting:AccessLifeTimeMinutes"));

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
                Expires = expired,

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(description);
            string tokenString = tokenHandler.WriteToken(token);
            //string tokenString = await Task.FromResult(tokenHandler.WriteToken(token));
            return new AccessToken { Token = tokenString, Created = create, Expired = expired};
        }

        public async Task<bool> IsValidated(string Token, string ipAddress)
        {
            var _isValid = _context.UserRefreshToken.FirstOrDefault(x => x.Token == Token && x.IpAddress == ipAddress) != null;
            return await Task.FromResult(_isValid);
        }
    }
}
