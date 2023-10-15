using MusicAPI.Data.Entities;

namespace MusicAPI.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetUser(int id);
        public User GetUser(string publicId);
        public User GetUserByLogin(string emailUser, string password);
        
    }
}
