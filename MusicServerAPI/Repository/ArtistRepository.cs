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

        public Artist GetArtist(int id)
        {
            return _dbContext.Artists.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Artist> GetArtists()
        {
            ICollection<Artist> artists =  _dbContext.Artists.ToList();
            return artists;
        }

        public void Update(Artist entity)
        {
            throw new NotImplementedException();
        }
    }
}
