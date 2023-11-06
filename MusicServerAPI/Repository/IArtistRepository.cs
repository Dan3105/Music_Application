using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public interface IArtistRepository : IRepository<Artist>
    {
        public Artist GetArtist(int id);
        public ICollection<Artist> GetArtists();
    }
}
