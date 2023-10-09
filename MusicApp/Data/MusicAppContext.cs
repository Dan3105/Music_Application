using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagesMusic.Users;

namespace MusicApp.Data
{
    public class MusicAppContext : DbContext
    {
        public MusicAppContext (DbContextOptions<MusicAppContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPagesMusic.Users.User> User { get; set; } = default!;
    }
}
