using MusicAPI.Data.Entities;

namespace MusicAPI.Repository.Interface
{
    public interface ITrackRepository : IRepository<Track>
    {
        public ICollection<Track> GetAll();
        public Track GetTrack(int id);
    }
}
