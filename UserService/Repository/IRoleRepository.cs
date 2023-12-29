using UserService.Entity;

namespace UserService.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task<ICollection<Role>> GetAll();
        public Task<ICollection<Role>> GetSubRoles(IEnumerable<int> ids);
    }
}
