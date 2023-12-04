using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
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
        Task<ICollection<Song>> GetSongsByUserAccount(User user);
        Task<IEnumerable<Song>> GetSongsByListId(IEnumerable<int> listId);  

    }
}
