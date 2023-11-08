using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicServerAPI.Entity
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Email cannot be longer than 150 characters")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        public bool Is_staff { get; set; } = false;

        public bool Is_superuser { get; set; } = false;

        public bool Is_activate { get; set; } = true;

        [AllowNull]
        public string? RefreshToken { get; set; }

        [Required]
        public DateTime CreatedTokenDate { get; set; }

        [Required]
        public DateTime ExpirationTokenDate { get; set; }

        public virtual ICollection<Playlist>? Playlists { set; get; }
        

        public virtual ICollection<UserRole>? UserRoles { set; get; }
        public virtual ICollection<Role>? Roles { set; get; }
    }
}
