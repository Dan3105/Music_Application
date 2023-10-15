using MusicAPI.Data;
using MusicAPI.Data.Entities;
using MusicAPI.Repository.Interface;

namespace MusicAPI.Repository
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly MusicAPIContext _context;
        public UserRolesRepository(MusicAPIContext context)
        {
            _context = context;
        }

        public bool Create(UserRoles entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(UserRoles entity)
        {
            throw new NotImplementedException();
        }

        public ICollection<string> GetAllRolesNameFromUser(int Id)
        {
            if(_context.UserRoles == null)
            {
                return null;
            }

            return _context.UserRoles.Where(x => x.UserId == Id)
                .Select(x => x.Role.RoleName)
                .ToList();
        }

        public bool Update(UserRoles entity)
        {
            throw new NotImplementedException();
        }
    }
}
