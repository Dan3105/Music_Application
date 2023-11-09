using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        public MusicServerAPIContext _dbContext;
        public ArtistRepository(MusicServerAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(Artist entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Artist> GetArtist(int id)
        {
            return await _dbContext.Artists.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Artist> GetArtistFetchSong(int id)
        {
            return await _dbContext.Artists
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Artist>> GetArtists()
        {
            ICollection<Artist> artists =  await _dbContext.Artists.ToListAsync();
            return artists;
        }

        public void Update(Artist entity)
        {
            throw new NotImplementedException();
        }
    }
}
