using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MusicAPI.Models;
[Table("track_playlist")]
[PrimaryKey(nameof(Track_id), nameof(Playlist_id))]
public class TrackPlaylist
{ 
    [Column("track_id")]
    public int Track_id { get; set; }

    [Column("playlist_id")]
    public int Playlist_id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Added { get; set; } = DateTime.Now;

    public Track? Track { get; set; }

    public Playlist? Playlist { get; set; }
}

