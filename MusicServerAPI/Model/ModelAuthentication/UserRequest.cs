namespace MusicServerAPI.Model.ModelAuthentication
{
    public class UserRequest
    {
        public string? UserEmail { set; get; }
        public string[]? Roles { set; get; }
    }
}
