using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MusicAPI.Data.Entities;

[Table("user_account")]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("public_id")]
    public Guid Public_id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(150, ErrorMessage = "Email cannot be longer than 150 characters")]
    [DataType(DataType.EmailAddress)]
    [Column("email")]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    [Column("password")]
    public string? password { get; set; }

    [DataType(DataType.DateTime)]
    [Column("created")]
    public DateTime Created { get; set; } = DateTime.Now;

    [Column("is_staff")]
    public bool Is_staff { get; set; } = false;

    [Column("is_superuser")]
    public bool Is_superuser { get; set; } = false;

    [Column("is_activate")]
    public bool Is_activate { get; set; } = true;

    public ICollection<Playlist>? playlists { get; set; }
    public ICollection<UserRefreshToken>? UserRefreshTokens { get; set; }
    public ICollection<UserRoles> UserRoles { get; set; }
}