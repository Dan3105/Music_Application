using MusicManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Repsitory
{
    public interface IRepoSongs
    {
        public Task<IEnumerable<Song>> GetSongs();
        public Task<Song> GetSong(int id);
        public Task AddSong(Song song);
        public Task UpdateSong(Song song);
        public Task DeleteSong(Song song);
    }
}
