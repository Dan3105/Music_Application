using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicAPI.Data.Entities
{
    [Table("roles")]
    [Index(nameof(RoleName), IsUnique = true)]
    public class Roles
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(20)]
        [Column("role_name")]
        public string RoleName { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
