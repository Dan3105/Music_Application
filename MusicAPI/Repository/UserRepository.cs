using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Data.Entities;
using MusicAPI.Models;
using MusicAPI.Repository.Interface;

namespace MusicAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MusicAPIContext _dbContext;

        public UserRepository(MusicAPIContext dbContext)
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
                _dbContext.User.Add(user);
            }
            catch (Exception e)
            {
                if (Utlity.Utility.IsInDebugMode())
                    Console.WriteLine(e);
                return false;
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public bool Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string publicId)
        {
            throw new NotImplementedException();
        }

        public User GetUserByLogin(string emailUser, string password)
        {
            throw new NotImplementedException();
        }

        public bool Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
