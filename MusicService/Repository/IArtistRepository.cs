using MusicService.Entity;

namespace MusicService.Repository
{
    public interface IArtistRepository : IRepository<Artist>
    {
        public Task<Artist> GetArtist(int id);
        public Task<ICollection<Artist>> GetArtists();
        public Task<Artist> GetArtistFetchSong(int id);
        public Task<IEnumerable<Artist>> GetSubArtists(IEnumerable<int> ids);   
        public bool CreateArtist(Artist artist);    
    }
}
