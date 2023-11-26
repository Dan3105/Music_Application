using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task<ICollection<Role>> GetAll();
        public Task<ICollection<Role>> GetSubRoles(IEnumerable<int> ids);
    }
}
