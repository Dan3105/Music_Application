using MusicServerAPI.Entity;

namespace MusicServerAPI.Model
{
    public class AlbumDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ArtisteDTO Artiste { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public ICollection<SongDTO> Songs { get; set; }

        public AlbumDTO() { }

        public AlbumDTO(Album album)
        {
            Id = album.Id;
            Name = album.Name;
            ImageUrl = album.ImageURL;
            ReleaseDate = album.ReleaseDate;

            if (album.Artist != null)
            {
                Artiste = new ArtisteDTO(album.Artist);
            }

            Songs = new List<SongDTO>();
            if (album.Songs != null)
            {
                foreach (var song in album.Songs)
                {
                    Songs.Add(new SongDTO(song));
                }
            }

        }
    }
}
