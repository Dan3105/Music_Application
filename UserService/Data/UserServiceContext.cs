using Microsoft.EntityFrameworkCore;
using UserService.Entity;
using UserService.Model;

namespace UserService.Data
{
    public class UserServiceContext : DbContext
    {
        public UserServiceContext(DbContextOptions<UserServiceContext> options) 
            : base(options)
        {
            Database.Migrate();
        }


        public DbSet<UserRole>? UserRoles { get; set; } = default;
        public DbSet<Role>? Roles { get; set; } = default;
        public DbSet<User>? Users { get; set; } = default;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
