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
                SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public bool Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<User>> GetAll()
        {
            return await _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(userRoles => userRoles.Role)
                .ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _dbContext.Users?
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.FavoriteSongs)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUser(string email)
        {
            return await _dbContext.Users?
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.FavoriteSongs)
                    .ThenInclude(fv => fv.Song)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserByLogin(string emailUser, string password)
        {
            var user = await GetUser(emailUser);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    return user;
                }
            }
            return null;
        }

        public Task<User> GetUserByToken(string token)
        {
            return _dbContext.Users.FirstOrDefaultAsync(p => p.RefreshToken == token);
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
                Console.WriteLine(e);
                return false;
            }
        }

        public bool Update(User entity)
        {
            _dbContext.Users.Update(entity);
            SaveChanges();
            return true;
        }
    }
}
