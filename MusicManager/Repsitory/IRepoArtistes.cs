﻿using MusicManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Repsitory
{
    public interface IRepoArtistes
    {
        public Task<IEnumerable<Artist>> GetArtistsAsync();
        public Task<Artist> GetArtistWithSongsAsync(Artist artist);
        public Task AddArtist(object image, Artist artist);
        public Task UpdateArtist(object image, Artist artist);
        public Task DeleteArtist(Artist artist);
    }
}
