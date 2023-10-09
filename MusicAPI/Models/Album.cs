using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicAPI.Models;

[Table("album")]
public class Album
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(50, ErrorMessage = "Album name cannot be longer than 50 characters")]
    public string? Title { get; set; }

    [Column("description", TypeName = "text")]
    [AllowNull]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Column("release_date")]
    public DateTime? Release_date { get; set; }

    [AllowNull]
    [Column("image")]
    [DataType(DataType.Url)]
    public string? Image { get; set; }

    [Required]
    [Column("artist")]
    public Artist? Artist { get; set; }

    [Required]
    [Column("genres")]
    public string? Genres { get; set; } = "";

    public ICollection<Track>? tracks { get; set; }
}

