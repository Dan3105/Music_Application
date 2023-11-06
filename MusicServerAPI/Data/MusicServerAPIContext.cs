using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Entity;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Artist>()
                .HasMany(e => e.Songs)
                .WithMany(e => e.Artists)
                .UsingEntity<ArtistSong>();

            modelBuilder.Entity<Song>()
                .HasMany(e => e.Artists)
                .WithMany(e => e.Songs)
                .UsingEntity<ArtistSong>();

            modelBuilder.Entity<Playlist>()
                .HasMany(e => e.Songs)
                .WithMany(e => e.Playlists)
                .UsingEntity<PlaylistSong>();

            modelBuilder.Entity<Song>()
                .HasMany(e => e.Playlists)
                .WithMany(e => e.Songs)
                .UsingEntity<PlaylistSong>();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .UsingEntity<UserRole>();

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .UsingEntity<UserRole>();
        }
    }
}
