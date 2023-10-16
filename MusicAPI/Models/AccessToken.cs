namespace MusicAPI.Models
{
    public class AccessToken
    {
        public string Token { set; get; }
        public DateTime Created { set; get; }
        public DateTime Expired { set; get; }
    }
}
