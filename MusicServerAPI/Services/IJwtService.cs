using MusicServerAPI.Model.ModelAuthentication;

namespace MusicServerAPI.Services
{
    public interface IJwtService
    {
        Task<AuthResponse> GetTokenAsync(UserRequest request);
        Task<AuthResponse> GetRefreshTokenAsync(UserRequest request);

        Task<bool> IsValidated(string Token, string ipAddress);
    }
}
