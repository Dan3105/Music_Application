using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;

namespace MusicServerAPI.Data
{
    public class MusicServerAPIContext : DbContext
    {
        public MusicServerAPIContext(DbContextOptions<MusicServerAPIContext> options) 
            : base(options)
        {
        }


        public DbSet<Artist>? Artists { get; set; } = default;
        public DbSet<UserRole>? UserRoles { get; set; } = default;
        public DbSet<ArtistSong>? ArtistSongs { get; set; } = default;

        public DbSet<PlaylistSong>? PlaylistSongs { get; set; } = default;
        public DbSet<Playlist>? Playlists { get; set; } = default;
        public DbSet<Role>? Roles { get; set; } = default;
        public DbSet<Song>? Songs { get; set; } = default;
        public DbSet<User>? Users { get; set; } = default;
        public DbSet<Album>? Albums { get; set; } = default;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
