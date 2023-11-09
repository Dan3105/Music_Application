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

            modelBuilder.Entity<Playlist>()  // create table Playlist
                .HasMany(e => e.Songs)       // table Playlist has many Song (show relationship between Playlist and SOng)
                .WithMany(e => e.Playlists)   // table Song has many Playlist (show relationsthip between Song adn Playlist)
                .UsingEntity<PlaylistSong>( // using table PlaylistSong to show relationship between Playlist and Song
                   j => j.HasOne(p => p.Song) // inside PlaylistSong we have reference to single Song
                    .WithMany(s => s.PlaylistSongs) // inside PlaylistSong we have reference to many Song
                    .HasForeignKey(p => p.SongId) // specificy foreing key in  table Playlistsong
                    .OnDelete(DeleteBehavior.Cascade),  // if song is delete, delete it in PlaylistSong too

                   j => j.HasOne(p => p.Playlist)
                    .WithMany(s => s.PlaylistSongs) 
                    .HasForeignKey(p => p.PlaylistId) 
                    .OnDelete(DeleteBehavior.Cascade)
                 );

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

            modelBuilder.Entity<User>()
                .HasMany(e => e.Songs)
                .WithMany(e => e.Users)
                .UsingEntity<FavoriteSongs>(
                    j => j.HasOne(p => p.Song)
                        .WithMany(p => p.FavoriteSongs)
                        .HasForeignKey(p => p.SongId)
                        .OnDelete(DeleteBehavior.Cascade),

                    j => j.HasOne(p => p.User)
                        .WithMany(p => p.FavoriteSongs)
                        .HasForeignKey(p => p.UserId)
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}
