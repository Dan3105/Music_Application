using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicAPI.Data.Entities;
[Table("user_refresh_token")]
public class UserRefreshToken
{
    [Key]
    public int id { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime ExpirationDate { get; set; }

    [NotMapped]
    public bool IsActive
    {
        get
        {
            return ExpirationDate > DateTime.UtcNow;
        }
    }

    public string IpAddress { get; set; }
    public bool IsInvalidated { get; set; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}

