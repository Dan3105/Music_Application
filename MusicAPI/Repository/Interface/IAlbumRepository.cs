using MusicAPI.Data.Entities;

namespace MusicAPI.Repository.Interface
{
    public interface IAlbumRepository : IRepository<Album>
    {
        public ICollection<Album> GetAllAlbum();
        public Album GetAlbum(int id);
    }
}
