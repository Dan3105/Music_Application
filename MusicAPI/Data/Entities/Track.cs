using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicAPI.Data.Entities;

[Table("track")]
public class Track
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(50, ErrorMessage = "Track name cannot be longer than 50 characters")]
    [MinLength(3, ErrorMessage = "Track name cannot be shorter than 3 characters")]
    public string? Title { get; set; }

    [AllowNull]
    [Column("release_date")]
    [DataType(DataType.DateTime)]
    public DateTime? Release_date { get; set; }

    [Required]
    [Column("duration")]
    public int Duration { get; set; } = 0;

    [Required]
    [Column("path_url")]
    public string? Path_url { get; set; } = "";

    [Required]
    [Column("genres")]
    public string? Genres { get; set; } = "";

    [Required]
    [Column("album")]
    public Album? Album { get; set; }

    public ICollection<TrackPlaylist>? playlists { get; set; }
}
