using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;
using System.Transactions;

namespace MusicServerAPI.Repository
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly MusicServerAPIContext _dbContext;

        public PlaylistRepository(MusicServerAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddPlaylist(Playlist entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Playlists.Add(entity);
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

        public bool Delete(Playlist entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Remove(entity);
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

        //Has include: User, PlaylistsSong, Song
        public async Task<Playlist> GetPlaylist(int id)
        {
            
            return await _dbContext.Playlists
                .Include(p => p.user)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(p => p.Song)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Playlist>> GetPlaylists()
        {
            //var user = await _dbContext.Users.Include(u => u.Playlists)
            //    .FirstOrDefaultAsync(u => u.Id == idUser);

            var playlists = await _dbContext.Playlists
                .Include(p => p.user)
                .Where(p => p.isPrivate == false).ToListAsync();
            return playlists;
        }

        public bool SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Update(Playlist entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    _dbContext.Playlists.Update(entity);
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
    }
}
