using System.ComponentModel.DataAnnotations;

namespace MusicServerAPI.Model.ModelAuthentication
{
    public class LoginRequest
    {
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
