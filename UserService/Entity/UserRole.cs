using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Entity
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public int UserId { set; get; }

        [Column("role_id")]
        [ForeignKey(nameof(Role))]
        public int RoleId { set; get; }

        public virtual User? User { set; get; }

        public virtual Role? Role { set; get; }
    }
}
