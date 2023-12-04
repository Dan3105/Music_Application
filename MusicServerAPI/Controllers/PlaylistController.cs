using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;
using MusicServerAPI.Repository;
using System.Security.Claims;

namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISongRepository _songRepository;

        public PlaylistController(IPlaylistRepository playlistRepository, IUserRepository userRepository, ISongRepository songRepository)
        {
            _playlistRepository = playlistRepository;
            _userRepository = userRepository;
            _songRepository = songRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            try { 
                var playlists = await _playlistRepository.GetPlaylists();
                ICollection<PlaylistDTO> playlistDTOs = new List<PlaylistDTO>();
                foreach (var playlist in playlists)
                {
                    playlistDTOs.Add(new PlaylistDTO(playlist));
                }

                return Ok(playlistDTOs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("Failed in Request");
            }   
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylist(int id)
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email);
                string email = "";
                if (emailClaim != null)
                {
                    email = emailClaim.Value;
                }

                if (email.Length == 0)
                {
                    return BadRequest("Fail Authorize");
                }
                var currentUserId = (await _userRepository.GetUser(email)).Id;
                var playlist = await _playlistRepository.GetPlaylist(id);

                if(playlist != null)
                {
                    var playlistDTO = new PlaylistDTO(playlist);
                    if(!playlist.isPrivate)
                    {
                        return Ok(playlistDTO);
                    }
                    return BadRequest("This Playlist is private");
                }

                return BadRequest("Failed in Request");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("Failed in Request");
            }

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePlaylist([FromBody] PlaylistRequestDTO form)
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email).Value;
                if (emailClaim == null)
                {
                    return BadRequest(400);
                }
                var currentUser = (await _userRepository.GetUser(emailClaim));

                Playlist playlist = new Playlist()
                {
                    Title = form.Title,
                    Description = form.Description,
                    isPrivate = form.IsPrivate,
                    user = currentUser,
                    PlaylistSongs = new List<PlaylistSong>()
                };

                var Songs = (await _songRepository.GetSongsByListId(form.SongIds.ToList())).Select(p => p.Id);
                    
                    //_context.Songs
                    //.Where(s => form.SongIds.Contains(s.Id))
                    //.ToList();


                foreach(var song in Songs)
                {
                    playlist.PlaylistSongs.Add(new PlaylistSong()
                    {
                        SongId = song
                    });
                }
                _playlistRepository.AddPlaylist(playlist);
                return Ok(playlist);
            }
            catch(Exception e) {
                Console.WriteLine($"{e.Message}");
                return BadRequest("Failed Request");
            }
        }

        [HttpPatch("edit/{id}")]
        public async Task<IActionResult> UpdatePlaylist([FromBody] PlaylistRequestDTO form, int id)
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email).Value;
                if (emailClaim == null)
                {
                    return BadRequest(400);
                }

                var currentUser = (await _userRepository.GetUser(emailClaim));


                Playlist playlist = await _playlistRepository.GetPlaylist(id);
                if (playlist == null || currentUser == null)
                {
                    return BadRequest("Failed in Request Playlist");
                }
                if(currentUser.Id != playlist.user?.Id)
                {
                    return BadRequest(400);
                }

                playlist.Title = form.Title;
                playlist.Description = form.Description;
                playlist.isPrivate = form.IsPrivate;

                //var Songs = _context.Songs
                //        .Where(s => form.SongIds.Contains(s.Id))
                //        .ToList();

                //playlist.PlaylistSongs.Select(ps => ps.Song) = Songs;

                _playlistRepository.Update(playlist);
                _playlistRepository.SaveChanges();

                return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Failed in Request");
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email).Value;
                if (emailClaim == null)
                {
                    return BadRequest(400);
                }

                var currentUser = (await _userRepository.GetUser(emailClaim));


                Playlist playlist = await _playlistRepository.GetPlaylist(id);
                if (playlist == null || currentUser == null)
                {
                    return BadRequest("Failed in Request Playlist");
                }
                if (currentUser.Id != playlist.user.Id)
                {
                    return BadRequest(400);
                }

                
                _playlistRepository.Delete(playlist);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Failed in Request");
            }

        }
    }
}