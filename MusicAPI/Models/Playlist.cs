using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicAPI.Models;

[Table("playlist")]
public class Playlist
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(50, ErrorMessage = "Playlist name cannot be longer than 50 characters")]
    public string? Title { get; set; }

    [AllowNull]
    [Column("description", TypeName ="text")]
    public string? Description { get; set; }

    [Required]
    public User? user { get; set; }

    public ICollection<TrackPlaylist>? tracks { get; set; }
}