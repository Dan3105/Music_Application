using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;

namespace MusicServerAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MusicServerAPIContext _dbContext;

        public UserRepository(MusicServerAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(RegisterDTO registerDTO)
        {
            User user = new User();
            user.Email = registerDTO.Email;
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
            user.password = hashPassword;
            try
            {
                _dbContext.Users.Add(user);
            }
            catch (Exception e)
            {
                //if (Utlity.Utility.IsInDebugMode())
                //    Console.WriteLine(e);
                return false;
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<User>> GetAll()
        {
            return await _dbContext.Users
                .Include(p => p.Roles)
                .ToListAsync();
        }

        public User GetUser(int id)
        {
            return _dbContext.Users?
                .Include(u => u.Roles)
                .Include(u => u.Songs)
                .FirstOrDefault(x => x.Id == id);
        }

        public User GetUser(string email)
        {
            return _dbContext.Users?
                .Include(u => u.Roles)
                .Include(u => u.Songs)
                .FirstOrDefault(x => x.Email == email);
        }

        public User GetUserByLogin(string emailUser, string password)
        {
            var user = GetUser(emailUser);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    return user;
                }
            }
            return null;
        }

        public bool SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //if (Utlity.Utility.IsInDebugMode())
                //    Console.WriteLine(e);
                return false;
            }
        }

        public void Update(User entity)
        {
            _dbContext.Users.Update(entity);
        }
    }
}
