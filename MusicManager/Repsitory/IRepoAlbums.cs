using MusicManager.Model;

namespace MusicManager.Repsitory
{
    public interface IRepoAlbums
    {
        public Task<IEnumerable<Album>> GetAlbums();
        public Task<Album> GetAlbumWithId(int id);
        public Task AddAlbum(object image, Album album);
        public Task UpdateAlbum(object image, Album album);
        public Task DeleteAlbum(Album album);
    }
}
