using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Entity
{
    [PrimaryKey(nameof(UserId), nameof(SongId))]
    public class FavoriteSongs
    {
        public int UserId { get; set; }

        [ForeignKey(nameof(Song))]
        public int SongId { get; set; }

        public virtual Song? Song { get; set; }
    }
}
