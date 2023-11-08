using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public interface IPlaylistRepository : IRepository<Playlist>
    {
        public Task<Playlist> GetPlaylist(int id);    
        public void AddPlaylist(Playlist entity);
        public Task<ICollection<Playlist>> GetPlaylists(int idUser);
        public void SaveChanges();
    }
}
