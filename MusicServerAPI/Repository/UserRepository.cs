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

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByLogin(string emailUser, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == emailUser);
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
