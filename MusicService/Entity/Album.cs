using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicService.Entity
{
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? ImageURL { get; set; }

        [Required]
        public Artist? Artist { get; set; }

        [AllowNull]
        [DataType(DataType.DateTime)]
        public DateTime? ReleaseDate { get; set; }

        public virtual ICollection<Song>? Songs { get; set; }
    }
}
