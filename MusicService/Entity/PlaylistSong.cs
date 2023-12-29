using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Entity
{
    [PrimaryKey(nameof(PlaylistId), nameof(SongId))]
    public class PlaylistSong
    {
        public virtual Playlist? Playlist {set; get;}
        [ForeignKey(nameof(Playlist))]
        public int PlaylistId { get; set; }

        public virtual Song? Song { set; get; }

        [ForeignKey(nameof(Song))]
        public int SongId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Added { get; set; } = DateTime.Now;
    }
}
