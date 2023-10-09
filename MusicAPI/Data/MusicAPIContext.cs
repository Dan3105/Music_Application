using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;

namespace MusicAPI.Data
{
    public class MusicAPIContext : DbContext
    {
        public MusicAPIContext (DbContextOptions<MusicAPIContext> options)
            : base(options)
        {
        }

        public DbSet<MusicAPI.Models.User> User { get; set; } = default!;

        public DbSet<MusicAPI.Models.Playlist> Playlist { get; set; } = default!;

        public DbSet<MusicAPI.Models.Artist> Artist { get; set; } = default!;

        public DbSet<MusicAPI.Models.Album> Album { get; set; } = default!;

        public DbSet<MusicAPI.Models.Track> Track { get; set; } = default!;

        public DbSet<MusicAPI.Models.TrackPlaylist> TrackPlaylist { get; set; } = default!;

        
    }
}
