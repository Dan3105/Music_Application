using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MusicAPI.Data.Entities;
[Table("track_playlist")]
[PrimaryKey(nameof(TrackId), nameof(PlaylistId))]
public class TrackPlaylist
{
    [Column("track_id")]
    [ForeignKey("Track")]
    public int TrackId { get; set; }

    [Column("playlist_id")]
    [ForeignKey("Playlist")]
    public int PlaylistId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Added { get; set; } = DateTime.Now;

    public virtual Track? Track { get; set; }
    public virtual Playlist? Playlist { get; set; }
}

