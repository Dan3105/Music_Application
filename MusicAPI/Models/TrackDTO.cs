namespace MusicAPI.Models
{
    public class TrackDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string coverImage { get; set; }
        public string ArtistName { get; set; }

        public TrackDTO(string id, string title, string coverImage, string artistName)
        {
            Id = id;
            Title = title;
            this.coverImage = coverImage;
            ArtistName = artistName;
        }
    }
}
