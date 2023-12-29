using Microsoft.EntityFrameworkCore;
using MusicService.Entity;
using MusicService.Model;

namespace MusicService.Data
{
    public class MusicServiceContext : DbContext
    {
        public MusicServiceContext(DbContextOptions<MusicServiceContext> options) 
            : base(options)
        {
            Database.Migrate();
        }


        public DbSet<Artist>? Artists { get; set; } = default;
        public DbSet<ArtistSong>? ArtistSongs { get; set; } = default;
        public DbSet<FavoriteSongs>? FavoriteSongs { get; set; } = default;
        public DbSet<PlaylistSong>? PlaylistSongs { get; set; } = default;
        public DbSet<Playlist>? Playlists { get; set; } = default;
        public DbSet<Song>? Songs { get; set; } = default;
        public DbSet<Album>? Albums { get; set; } = default;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
