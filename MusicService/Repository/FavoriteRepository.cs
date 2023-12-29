using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MusicService.Data;
using MusicService.Entity;

namespace MusicService.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly MusicServiceContext _dbContext;

        public FavoriteRepository(MusicServiceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(int userId, int songId)
        {
            FavoriteSongs fs = await _dbContext.FavoriteSongs.FirstOrDefaultAsync(p => p.UserId == userId && p.SongId == songId);
            if (fs == null)
            {
                _dbContext.Add(new FavoriteSongs { SongId=songId, UserId=userId});
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool Delete(Song entity)
        {
           throw new NotImplementedException();
        }

        public async Task<bool> Delete(int userId, int songId)
        {
            FavoriteSongs fs = await _dbContext.FavoriteSongs.FirstOrDefaultAsync(p => p.UserId == userId && p.SongId == songId);
            if (fs != null)
            {
                _dbContext.Remove(fs);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ICollection<Song>> GetFavoritesByUserId(int userId)
        {
            var listSongIds = await _dbContext.FavoriteSongs.Where(s => s.UserId == userId)
                .Include(fs => fs.Song)
                .ToListAsync();

            return listSongIds.Select(s => s.Song).ToList();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public bool Update(Song entity)
        {
            throw new NotImplementedException();
        }
    }
}
