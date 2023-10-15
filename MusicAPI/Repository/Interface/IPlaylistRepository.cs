using MusicAPI.Data.Entities;

namespace MusicAPI.Repository.Interface
{
    public interface IPlaylistRepository: IRepository<Playlist>
    {
        public ICollection<Playlist> GetAll();
        public Playlist GetPlaylist(int id);
    }
}
