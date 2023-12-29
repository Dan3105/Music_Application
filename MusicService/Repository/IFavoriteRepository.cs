using MusicService.Entity;

namespace MusicService.Repository
{
    public interface IFavoriteRepository : IRepository<Song>
    {
        public Task<ICollection<Song>> GetFavoritesByUserId(int userId);
        public Task<bool> Delete(int userId, int songId);
        public Task<bool> Add(int userId, int songId);
    }
}
