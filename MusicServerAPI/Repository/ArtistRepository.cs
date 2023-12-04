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

        public bool CreateArtist(Artist artist)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Add(artist);
                    SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool Delete(Artist entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Artists.Remove(entity);
                    SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.ToString());
                    return false;
                }

            }
        }

        public async Task<Artist> GetArtist(int id)
        {
            return await _dbContext.Artists.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Artist> GetArtistFetchSong(int id)
        {
            return await _dbContext.Artists
                .Include(a => a.ArtistSongs)
                .ThenInclude(ar => ar.Song)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Artist>> GetArtists()
        {
            ICollection<Artist> artists =  await _dbContext.Artists.ToListAsync();
            return artists;
        }

        public async Task<IEnumerable<Artist>> GetSubArtists(IEnumerable<int> ids)
        {
            return await _dbContext.Artists.Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }

        public bool SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool Update(Artist entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Update(entity);
                    SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    transaction.Rollback();
                    return false;
                }

            }
        }
    }
}
