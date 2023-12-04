using MusicServerAPI.Entity;

namespace MusicServerAPI.Model
{
    public class PlaylistDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
        public int UserId { get; set; }
        public ICollection<SongDTO> Songs { get; set; }

        public PlaylistDTO() { }

        public PlaylistDTO(Playlist playlist)
        {
            Id = playlist.Id;
            Title = playlist.Title;
            Description = playlist.Description;
            IsPrivate = playlist.isPrivate;
            UserId = playlist.user != null ? playlist.user.Id : -1;
            if (playlist.PlaylistSongs != null)
            {
                Songs = new List<SongDTO>();
                foreach (var plSong in playlist?.PlaylistSongs)
                {
                    var song = plSong.Song;
                    Songs.Add(new SongDTO(song));
                }

            }
        }
    }
}
