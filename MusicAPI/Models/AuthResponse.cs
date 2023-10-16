namespace MusicAPI.Models
{
    public class AuthResponse
    {
        public AccessToken AccessToken { get; set; }
        public bool IsSuccess { get; set; }
        public string Reason { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
