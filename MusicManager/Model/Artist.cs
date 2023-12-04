using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Model
{
    public class Artist
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("bio")]
        public string? Bio { get; set; }

        [JsonProperty("image")]
        public string? Image { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("songs")]
        public List<Song>? Songs { get; set; }
    }
}
