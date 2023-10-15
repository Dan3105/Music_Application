using MusicAPI.Data.Entities;

namespace MusicAPI.Repository.Interface
{
    public interface IArtistRepository : IRepository<Artist>
    {
        public ICollection<Artist> GetAllArtist();
        public Artist GetArtist(int id);
    }
}
