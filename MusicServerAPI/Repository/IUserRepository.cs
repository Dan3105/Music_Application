using MusicServerAPI.Entity;
using MusicServerAPI.Model;

namespace MusicServerAPI.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<bool> Create(RegisterDTO registerDTO);
        public Task<User> GetUser(int id);
        public Task<User> GetUser(string email);
        public Task<User> GetUserByToken(string token);
        public Task<User> GetUserByLogin(string emailUser, string password);
        public Task<ICollection<User>> GetAll();
    }
}
