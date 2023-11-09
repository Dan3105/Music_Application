using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public interface IArtistRepository : IRepository<Artist>
    {
        public Task<Artist> GetArtist(int id);
        public Task<ICollection<Artist>> GetArtists();
        public Task<Artist> GetArtistFetchSong(int id);
    }
}
