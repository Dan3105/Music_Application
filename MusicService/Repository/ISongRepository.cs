using MusicService.Entity;

namespace MusicService.Repository
{
    public interface ISongRepository : IRepository<Song>
    {
        Task<Song> GetSong(int id);
        bool CreateSong(Song song);
        Task<ICollection<Song>> GetSongs();

        Task<ICollection<Song>> GetSongsByArtistId(int artistId, int length=10);
        Task<ICollection<Song>> GetSongsOrderDateRealease(int length=10);
        Task<ICollection<Song>> GetSongsOrderLikes(int length = 10);
        Task<ICollection<Song>> GetSongsBySearch(string search, int length = 10);
        Task<ICollection<Song>> GetSongsByUserAccount(int user);
        Task<IEnumerable<Song>> GetSongsByListId(IEnumerable<int> listId);  

    }
}
