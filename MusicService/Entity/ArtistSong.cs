using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Entity
{
    [PrimaryKey(nameof(ArtistId), nameof(SongId))]
    public class ArtistSong
    {
        [ForeignKey(nameof(Artist))]
        public int ArtistId { get; set; }
        public virtual Artist? Artist { get; set; }

        [ForeignKey(nameof(Song))]
        public int SongId { get; set; }
        public virtual Song? Song { get; set; } 
    }
}
