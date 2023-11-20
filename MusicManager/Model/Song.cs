using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Model
{

    public class Song
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Duration { get; set; }
        public int Likes { get; set; }
        public string? CoverImage { get; set; }
        public string? SongURL { get; set; }
        public List<Artist>? Artists { get; set; }
    }
}
