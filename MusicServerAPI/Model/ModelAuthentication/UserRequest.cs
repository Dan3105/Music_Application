namespace MusicServerAPI.Model.ModelAuthentication
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string? UserEmail { set; get; }
        public string[]? Roles { set; get; }
    }
}
