using MusicAPI.Data.Entities;

namespace MusicAPI.Repository.Interface
{
    public interface IUserRolesRepository : IRepository<UserRoles>
    {
        public ICollection<string> GetAllRolesNameFromUser(int id);

    }
}
