namespace MusicAPI.Models
{
    public class TokenFormRequest
    {
        public int UserId { set; get; }
        public string IpAddress { set; get; }
        public string UserEmail { set; get; }
        public string[] Roles { set; get; }
    }
}
