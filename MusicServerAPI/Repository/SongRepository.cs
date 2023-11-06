using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly MusicServerAPIContext _dbContext;
        
        public SongRepository(MusicServerAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(Song entity)
        {
            throw new NotImplementedException();
        }

        public Song GetSong(int id)
        {
            return _dbContext.Songs.Include(s => s.Artists).FirstOrDefault(song => song.Id == id);
        }

        public async Task<ICollection<Song>> GetSongs()
        {
            return await _dbContext.Songs
                .Include(s => s.ArtistSongs)
                .Include(s => s.Artists)
                .ToListAsync();
        }

        public async Task<ICollection<Song>> GetSongsByArtistId(int artistId, int length = 10)
        {
            return await _dbContext.Artists
                .Include(s => s.ArtistSongs)
                .Include(s => s.Songs)
                .Where(s => s.Id == artistId)
                .SelectMany(s => s.Songs)
                .Take(length)
                .ToListAsync();


        }

        public async Task<ICollection<Song>> GetSongsOrderDateRealease(int length=10)
        {
            return await _dbContext.Songs
                .Include(s => s.ArtistSongs)
                .Include(s => s.Artists)
                .OrderByDescending(song => song.ReleaseDate)
                .Take(length)
                .ToListAsync();
        }

        public Task<ICollection<Song>> GetSongsBySearch(string search, int length = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Song>> GetSongsOrderLikes(int length=10)
        {
            return await _dbContext.Songs
                .Include(s => s.ArtistSongs)
                .Include(s => s.Artists)
                .OrderByDescending(song => song.Likes)
                .Take(length)
                .ToListAsync();
        }

        public void Update(Song entity)
        {
            throw new NotImplementedException();
        }
    }
}
