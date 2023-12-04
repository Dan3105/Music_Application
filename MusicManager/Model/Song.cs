using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Model
{
    public class SongSelectors
    {
       public Song song { set; get; }
       public bool isSelected { set; get; }
    }

    public class Song
    {
        [JsonProperty("id")]    
        public int Id { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("coverImage")]
        public string? CoverImage { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("songURL")]
        public string? SongURL { get; set; }

        [JsonProperty("artists")]
        public List<Artist>? Artists { get; set; }
    }
}
