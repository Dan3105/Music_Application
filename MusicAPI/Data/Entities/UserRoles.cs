using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicAPI.Data.Entities;

[Table("user_roles")]
[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRoles
{
    [Column("user_id")]
    [ForeignKey("User")]
    public int UserId { set; get; }

    [Column("role_id")]
    [ForeignKey("Role")]
    public int RoleId { set; get; }

    public virtual User User { set; get; }

    public virtual Roles Role { set; get; }
}
