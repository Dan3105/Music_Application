using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicServerAPI.Entity
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Playlist name cannot be longer than 50 characters")]
        public string? Title { get; set; }

        [AllowNull]
        [Column(TypeName = "text")]
        public string? Description { get; set; }

        [Required]
        public bool isPrivate { get; set; } = true;

        [Required]
        public User? user { get; set; }

        public virtual ICollection<PlaylistSong>? PlaylistSongs { get; set; }

    }
}
