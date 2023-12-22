using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Model
{
    public class Album
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("artiste")]
        public Artist Artiste { get; set; }
        [JsonProperty("releaseDate")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("songs")]
        public ICollection<Song> Songs { get; set; }
    }
}
