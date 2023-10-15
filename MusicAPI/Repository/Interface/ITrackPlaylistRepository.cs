using MusicAPI.Data.Entities;

namespace MusicAPI.Repository.Interface
{
    public interface ITrackPlaylistRepository : IRepository<TrackPlaylist>
    {
        public ICollection<Track> GetPlaylistFromUser(string publicId);
        
    }
}
