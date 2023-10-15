using MusicAPI.Models;

namespace MusicAPI.Services
{
    public interface IJwtService
    {
        Task<AuthResponse> GetTokenAsync(TokenFormRequest tokenForm);
        Task<AuthResponse> GetRefreshTokenAsync(TokenFormRequest tokenForm);

        Task<bool> IsValidated(string Token, string ipAddress);
    }
}
