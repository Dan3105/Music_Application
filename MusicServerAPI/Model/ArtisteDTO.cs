using MusicServerAPI.Entity;

namespace MusicServerAPI.Model
{
    public class ArtisteDTO
    {
        public int id { set; get; }
        public string? name { set; get; }
        public string? bio { set; get; }

        public string? image { set; get; }
        public string type { set; get; } = "Artiste";

        public ArtisteDTO(Artist artist)
        {
            id = artist.Id;
            name = artist.Name;
            bio = artist.Biography;
            image = artist.Image;
            type = "Artiste";
        }
    }
}
