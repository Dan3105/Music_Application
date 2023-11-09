using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicServerAPI.Entity
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Track name cannot be longer than 50 characters")]
        [MinLength(3, ErrorMessage = "Track name cannot be shorter than 3 characters")]
        public string? Title { get; set; }

        [AllowNull]
        public string? CoverImage { get; set; }

        [AllowNull]
        [DataType(DataType.DateTime)]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        public int Duration { get; set; } = 0;

        [Required]
        public int Likes { get; set; } = 0;

        [Required]
        public string? SongURL { get; set; } = "";

        public virtual ICollection<ArtistSong>? ArtistSongs { get; }
        public virtual ICollection<Artist>? Artists { get; }
        public virtual ICollection<PlaylistSong>? PlaylistSongs { get; }
        public virtual ICollection<Playlist>? Playlists { get; }
    
        public virtual ICollection<FavoriteSongs>? FavoriteSongs { get; }
        public virtual ICollection<User>? Users { get; }
    }
}
