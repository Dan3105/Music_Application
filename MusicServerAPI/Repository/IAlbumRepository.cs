using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public interface IAlbumRepository : IRepository<Album>
    {
        public Task<Album> GetAlbum(int id);
        public Task<ICollection<Album>> GetAlbums();
        public bool CreateAlbum(Album album);
    }
}
