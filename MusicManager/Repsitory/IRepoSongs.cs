using MusicManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MusicManager.Repsitory
{
    public interface IRepoSongs
    {
        Task<IEnumerable<Song>> GetSongs();
        Task<Song> GetSong(int id);

        Task DeleteSong(Song song);

        Task AddSong(object imageSource, string media, Song song);
        Task UpdateSong(object imageSource, string media, Song song);
    }
}
