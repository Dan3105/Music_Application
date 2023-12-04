using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public interface IPlaylistRepository : IRepository<Playlist>
    {
        public Task<Playlist> GetPlaylist(int id);    
        public bool AddPlaylist(Playlist entity);
        public Task<ICollection<Playlist>> GetPlaylists();
    }
}
