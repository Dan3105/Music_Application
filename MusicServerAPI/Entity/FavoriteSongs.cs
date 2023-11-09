using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicServerAPI.Entity
{
    [PrimaryKey(nameof(UserId), nameof(SongId))]
    public class FavoriteSongs
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(Song))]
        public int SongId { get; set; }

        public virtual User? User { get; set; }
        public virtual Song? Song { get; set; }
    }
}
