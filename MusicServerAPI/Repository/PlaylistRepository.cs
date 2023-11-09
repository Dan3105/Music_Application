using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly MusicServerAPIContext _dbContext;

        public PlaylistRepository(MusicServerAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPlaylist(Playlist entity)
        {
            try
            {
                _dbContext.Playlists.Add(entity);
            }
            catch(Exception e) 
            { 
                Console.WriteLine(e.ToString());
            }
            
        }

        public void Delete(Playlist entity)
        {
            _dbContext.Remove(entity);
        }

        //Has include: User, PlaylistsSong, Song
        public async Task<Playlist> GetPlaylist(int id)
        {
            
            return await _dbContext.Playlists
                .Include(p => p.user)
                .Include(p => p.PlaylistSongs)
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Playlist>> GetPlaylists(int idUser)
        {
            var user = await _dbContext.Users.Include(u => u.Playlists)
                .FirstOrDefaultAsync(u => u.Id == idUser);
            return user.Playlists?.ToList();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Playlist entity)
        {
            _dbContext.Playlists.Update(entity);
        }
    }
}
