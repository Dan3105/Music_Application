using Microsoft.AspNetCore.Mvc;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;
using MusicServerAPI.Repository;

namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : Controller
    {
        private readonly ISongRepository _songRepository;

        public SongController(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        [HttpGet]
        public async Task<ICollection<SongDTO>> GetSongs()
        {
            ICollection<Song> songs = await _songRepository.GetSongs();
            List<SongDTO> songList = new List<SongDTO>();
            foreach (var song in songs)
            {
                songList.Add(new SongDTO(song));
            }
            return songList;
        }

        [HttpGet("by-artist-{id}")]
        public async Task<ICollection<SongDTO>> GetSongsByArtist(int id)
        {
            ICollection<Song> songs = await _songRepository.GetSongsByArtistId(id);
            List<SongDTO> songList = new List<SongDTO>();

            foreach (var song in songs)
            {
                songList.Add(new SongDTO(song));
            }

            return songList;
        }

        [HttpGet("most-liked")]
        public async Task<ICollection<SongDTO>> GetMostLikeSong()
        {
            ICollection<Song> songs = await _songRepository.GetSongsOrderLikes();
            List<SongDTO> songList = new List<SongDTO>();

            foreach (var song in songs)
            {
                songList.Add(new SongDTO(song));
            }

            return songList;
        }

        [HttpGet("latest")]
        public async Task<ICollection<SongDTO>> GetLatestSong()
        {
            ICollection<Song> songs = await _songRepository.GetSongsOrderDateRealease();
            List<SongDTO> songList = new List<SongDTO>();

            foreach(var song in songs)
            {
                songList.Add(new SongDTO(song));
            }
            return songList;
        }
    }
}
