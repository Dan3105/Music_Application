namespace MusicServerAPI.Model
{
    public class PlaylistRequestDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
        public int[]? SongIds { get; set; }

        public PlaylistRequestDTO() { } 
    }
}
