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

        public ICollection<SongDTO> Songs { set; get; }

        public ArtisteDTO(int id, string? name, string? bio, string? image, string type)
        {
            this.id = id;
            this.name = name;
            this.bio = bio;
            this.image = image;
            this.type = type;
        }

        public ArtisteDTO(Artist artist)
        {
            id = artist.Id;
            name = artist.Name;
            bio = artist.Biography;
            image = artist.Image;
            type = "Artiste";

            if(artist.Songs != null)
            {
                Songs = new List<SongDTO>();
                foreach(var song in artist.Songs)
                {
                    SongDTO customSong = new SongDTO
                    (
                        song.Id,
                        song.Title,
                        song.Duration,
                        song.Likes,
                        song.CoverImage,
                        song.SongURL
                    );
                    Songs.Add(customSong);
                }
            }
        }
    }
}
