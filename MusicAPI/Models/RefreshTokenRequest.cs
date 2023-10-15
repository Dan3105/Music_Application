using System.ComponentModel.DataAnnotations;

namespace MusicAPI.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string ExpiredToken { set; get; }

        [Required]
        public string RefreshToken { set; get; }


    }
}
