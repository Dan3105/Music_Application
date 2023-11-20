using MusicServerAPI.Entity;
using MusicServerAPI.Model;

namespace MusicServerAPI.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<bool> Create(RegisterDTO registerDTO);
        public User GetUser(int id);
        public User GetUser(string email);
        public User GetUserByLogin(string emailUser, string password);
        public Task<ICollection<User>> GetAll();
        public void Update(User entity);
        public bool SaveChanges();
    }
}
