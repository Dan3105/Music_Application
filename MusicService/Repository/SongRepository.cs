using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Entity;

namespace MusicService.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly MusicServiceContext _dbContext;
        
        public SongRepository(MusicServiceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Delete(Song entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Songs.Remove(entity);
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

        public async Task<Song> GetSong(int id)
        {
            return await _dbContext.Songs
                .Include(s => s.ArtistSongs)
                    .ThenInclude(artistSong => artistSong.Artist)
                .FirstOrDefaultAsync(song => song.Id == id);
        }

        public async Task<ICollection<Song>> GetSongs()
        {
            return await _dbContext.Songs
                .Include(s => s.ArtistSongs)
                    .ThenInclude(ars => ars.Artist)
                .ToListAsync();
        }

        public async Task<ICollection<Song>> GetSongsByArtistId(int artistId, int length = 20)
        {
            var artist = await _dbContext.Artists
                .Include(s => s.ArtistSongs)
                    .ThenInclude(artistSong => artistSong.Song)
                .FirstOrDefaultAsync(s => s.Id == artistId);

            return artist.ArtistSongs.Select(p => p.Song).ToList();
        }

        public async Task<ICollection<Song>> GetSongsOrderDateRealease(int length=20)
        {
            return await _dbContext.Songs
                .Include(s => s.ArtistSongs)
                    .ThenInclude(ars => ars.Artist)
                .OrderByDescending(song => song.ReleaseDate)
                .Take(length)
                .ToListAsync();
        }

        public async Task<ICollection<Song>> GetSongsBySearch(string search, int length = 20)
        {
            return await _dbContext.Songs
               .Include(s => s.ArtistSongs)
                   .ThenInclude(ars => ars.Artist)
               .Where(s => s.Title.ToLower().Contains(search.ToLower()))
               .ToListAsync();
        }

        public async Task<ICollection<Song>> GetSongsOrderLikes(int length=20)
        {
            return await _dbContext.Songs
                .Include(s => s.ArtistSongs)
                    .ThenInclude(s => s.Artist)
                .OrderByDescending(song => song.Likes)
                .Take(length)
                .ToListAsync();
        }

        public bool Update(Song entity)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //_dbContext.Entry(entity).State = EntityState.Modified;
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

        public async Task<ICollection<Song>> GetSongsByUserAccount(int user)
        {
            var songs = await _dbContext.Songs
                                    .Include(p => p.ArtistSongs)
                                        .ThenInclude(ars => ars.Artist)
                                    .Include(p => p.FavoriteSongs)
                                    .Where(s => s.FavoriteSongs.Any(fv => fv.UserId == user))
                                    .ToListAsync();
            return songs;
        }

        public bool CreateSong(Song song)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //_dbContext.Entry(song).State = EntityState.Added;
                    _dbContext.Add(song);
                    SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    transaction.Rollback();
                    return false;
                }
            }

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

        public async Task<IEnumerable<Song>> GetSongsByListId(IEnumerable<int> listId)
        {
           return await _dbContext.Songs.Where(p => listId.Contains(p.Id)).ToListAsync();
        }
    }
}
