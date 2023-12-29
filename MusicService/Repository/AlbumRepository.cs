using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Entity;

namespace MusicService.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        public MusicServiceContext _dbContext;
        public AlbumRepository(MusicServiceContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CreateAlbum(Album album)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Add(album);
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

        public bool Delete(Album entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Albums.Remove(entity);
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

        public async Task<Album> GetAlbum(int id)
        {
            return await _dbContext.Albums
                .Include(al => al.Artist)
                .Include(al => al.Songs)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Album>> GetAlbums()
        {
            return await _dbContext.Albums
               .Include(al => al.Artist)
               .Include(al => al.Songs)
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

        public bool Update(Album entity)
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
