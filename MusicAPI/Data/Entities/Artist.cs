using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicAPI.Data.Entities;

[Table("artist")]
public class Artist
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Artist name cannot be longer than 50 characters")]
    [Column("name")]
    public string? Name { get; set; }

    [AllowNull]
    [Column("biography", TypeName = "text")]
    public string? Biography { get; set; }

    [AllowNull]
    [Column("image")]
    [DataType(DataType.Url)]
    public string? Image { get; set; }

    public ICollection<Album>? albums { get; set; }
}
