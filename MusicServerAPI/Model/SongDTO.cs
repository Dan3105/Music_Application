using MusicServerAPI.Entity;

namespace MusicServerAPI.Model
{
    public class SongDTO
    {
        public int id { get; set; }
        public string? title { get; set; }
        public int duration { get; set; }
        public int likes { get; set; }
        public string? coverImage { get; set; }
        public string? songURL { get; set; }
        public ICollection<ArtisteDTO> artists { get; set; } = default;

        public SongDTO() {}
        
        public SongDTO(int _id, string title, int duration, int likes, string coverImage, string songURL)
        {
            this.id = _id;
            this.title = title;
            this.duration = duration;
            this.likes = likes;
            this.coverImage = coverImage;
            this.songURL = songURL;
        }
        public SongDTO(Song song)
        {
            this.artists = new List<ArtisteDTO>();
            this.id = song.Id;
            this.title = song.Title;
            this.duration = song.Duration;
            this.likes = song.Likes;
            this.coverImage = song.CoverImage;
            this.songURL = song.SongURL;
            if (song.Artists != null)
            {
                foreach (var artist in song?.Artists)
                {
                    this.artists.Add(new ArtisteDTO(
                        artist.Id, artist.Name, artist.Biography, artist.Image, "Artiste"
                        ));
                }
            }
            else
            {
                //Console.WriteLine($"{song.Id}:{song.Title} don't fetch artist");
            }
        }


    }
}
