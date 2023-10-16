using MusicAPI.Data.Entities;
using MusicAPI.Models;

namespace MusicAPI.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<bool> Create(RegisterDTO registerDTO);
        public User GetUser(int id);
        public User GetUser(string publicId);
        public User GetUserByLogin(string emailUser, string password);
        
    }
}
