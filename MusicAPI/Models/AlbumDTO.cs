namespace MusicAPI.Models
{
    public class AlbumDTO
    {
        public ICollection<TrackDTO> trackDTOs { set; get; }
        
        public string Title { set; get; }

    }
}
