using MusicServerAPI.Entity;
using MusicServerAPI.Model;

namespace MusicServerAPI.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<bool> Create(RegisterDTO registerDTO);
        public User GetUser(int id);
        public User GetUserByLogin(string emailUser, string password);
        public void Update(User entity);
        public bool SaveChanges();
    }
}
