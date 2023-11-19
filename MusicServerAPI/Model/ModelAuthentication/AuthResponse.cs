namespace MusicServerAPI.Model.ModelAuthentication
{
    public class AuthResponse
    {
        public AccessToken? AccessToken { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? Reason { get; set; }
        public RefreshToken? RefreshToken { get; set; }
        public UserRequest? UserRequest { get; set; }
    }
}
