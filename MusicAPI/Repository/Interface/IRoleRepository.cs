using MusicAPI.Data.Entities;

namespace MusicAPI.Repository.Interface
{
    public interface IRoleRepository : IRepository<Roles>
    {   
        public ICollection<Roles> GetAll();
        public Roles GetRole(int id);
    }
}
