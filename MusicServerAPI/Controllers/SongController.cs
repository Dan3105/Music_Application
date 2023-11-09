using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;
using MusicServerAPI.Model.ModelAuthentication;
using MusicServerAPI.Repository;
using System.Security.Claims;

namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : Controller
    {
        private readonly ISongRepository _songRepository;
        private readonly IUserRepository _userRepository;

        public SongController(ISongRepository songRepository, IUserRepository userRepository)
        {
            _songRepository = songRepository;
            _userRepository = userRepository;
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

        [Authorize]
        [HttpPatch("like/{id}")]
        public async Task<IActionResult> PostLikeMusic(int id)
        {
            try
            {
                string emailClaim = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (emailClaim == null)
                {
                    return BadRequest(400);
                }
                User user = _userRepository.GetUser(emailClaim); //_context.Users.FirstOrDefault(x => x.Email == emailClaim);
     
                if (user.Songs.Select(p => p.Id).Contains(id))
                {
                    user.Songs = user.Songs.Where(p => p.Id != id).ToList();
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                }
                else
                {
                    Song song = await _songRepository.GetSong(id);

                    user.Songs.Add(song);

                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                }

                UserRequest userRequest = new UserRequest()
                {
                    UserEmail = emailClaim,
                    Roles = user.Roles.Select(r => r.RoleName).ToArray(),
                    Favorites = user.Songs.Select(p => p.Id).ToArray()
                };
                return Ok(userRequest);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(400);
            }
        }
    }
}
