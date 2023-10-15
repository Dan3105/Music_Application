using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data.Entities;

namespace MusicAPI.Data
{
    public class MusicAPIContext : DbContext
    {
        public MusicAPIContext (DbContextOptions<MusicAPIContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;
       
        public DbSet<Playlist> Playlist { get; set; } = default!;

        public DbSet<Artist> Artist { get; set; } = default!;

        public DbSet<Album> Album { get; set; } = default!;

        public DbSet<Track> Track { get; set; } = default!;

        public DbSet<TrackPlaylist> TrackPlaylist { get; set; } = default!;

        public DbSet<MusicAPI.Models.UserRefreshToken> UserRefreshToken { get; set; } = default!;
        public DbSet<Roles> Roles { get; set; } = default!;
        public DbSet<UserRoles> UserRoles { get; set; } = default!;

    }
}
