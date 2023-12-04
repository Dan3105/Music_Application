using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;

namespace MusicServerAPI.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MusicServerAPIContext _dbContext;
        public RoleRepository(MusicServerAPIContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Delete(Role entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Role>> GetAll()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<ICollection<Role>> GetSubRoles(IEnumerable<int> ids)
        {
            return await _dbContext.Roles.Where(r => ids.Contains(r.Id)).ToListAsync();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public bool Update(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
