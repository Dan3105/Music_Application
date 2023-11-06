using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicServerAPI.Entity
{
    [Index(nameof(RoleName), IsUnique = true)]
    public class Role
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(20)]
        [Column("role_name")]
        public string? RoleName { get; set; }

        public virtual ICollection<UserRole>? UserRoles { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
