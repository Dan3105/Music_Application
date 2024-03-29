﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicService.Entity
{
    public class Artist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Artist name cannot be longer than 50 characters")]
        public string? Name { get; set; }

        [AllowNull]
        public string? Biography { get; set; }

        [AllowNull]
        [DataType(DataType.Url)]
        public string? Image { get; set; }

        public virtual ICollection<ArtistSong>? ArtistSongs { get; set; }
    }
}
