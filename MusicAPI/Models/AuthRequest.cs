using System.ComponentModel.DataAnnotations;

namespace MusicAPI.Models
{
    public class AuthRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
