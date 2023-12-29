using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Entity;
using MusicService.Model;
using MusicService.Repository;
using System.Security.Claims;

namespace MusicService.Controllers
{
    [Route("api/MusicService/[controller]")]
    [Authorize]
    [ApiController]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistRepository _playlistRepository;
        //private readonly IUserRepository _userRepository;
        private readonly ISongRepository _songRepository;

        public PlaylistController(IPlaylistRepository playlistRepository, ISongRepository songRepository)
        {
            _playlistRepository = playlistRepository;
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
                string idClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (idClaim == null)
                {
                    return BadRequest(400);
                }

                if(int.TryParse(idClaim, out int idClaimConvert))
                {
                    var playlist = await _playlistRepository.GetPlaylist(id);

                    if (playlist != null)
                    {
                        var playlistDTO = new PlaylistDTO(playlist);
                        if (!playlist.isPrivate)
                        {
                            return Ok(playlistDTO);
                        }
                        else
                        {
                            if(playlist.user == idClaimConvert)
                            {
                                return Ok(playlistDTO);
                            }
                        }
                        return BadRequest("This Playlist is private");
                    }
                }
                else
                {
                    return BadRequest(400);
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
                string idClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (idClaim == null)
                {
                    return BadRequest(400);
                }

                if (int.TryParse(idClaim, out int idClaimConvet))
                {
                    Playlist playlist = new Playlist()
                    {
                        Title = form.Title,
                        Description = form.Description,
                        isPrivate = form.IsPrivate,
                        user = idClaimConvet,
                        PlaylistSongs = new List<PlaylistSong>()
                    };

                    var Songs = (await _songRepository.GetSongsByListId(form.SongIds.ToList())).Select(p => p.Id);


                    foreach (var song in Songs)
                    {
                        playlist.PlaylistSongs.Add(new PlaylistSong()
                        {
                            SongId = song
                        });
                    }
                    _playlistRepository.AddPlaylist(playlist);
                }

                return Ok();

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
                string idClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (idClaim == null)
                {
                    return BadRequest(400);
                }

                if(int.TryParse(idClaim, out int idClaimConvet))
                {
                    Playlist playlist = await _playlistRepository.GetPlaylist(id);
                    if (playlist == null)
                    {
                        return BadRequest("Failed in Request Playlist");
                    }
                    if (idClaimConvet != playlist.user)
                    {
                        return BadRequest(400);
                    }

                    playlist.Title = form.Title;
                    playlist.Description = form.Description;
                    playlist.isPrivate = form.IsPrivate;


                    _playlistRepository.Update(playlist);
                    _playlistRepository.SaveChanges();

                }

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
                string idClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (idClaim == null)
                {
                    return BadRequest(400);
                }

                if (int.TryParse(idClaim, out int idClaimConvet))
                {
                    Playlist playlist = await _playlistRepository.GetPlaylist(id);
                    if (playlist == null)
                    {
                        return BadRequest("Failed in Request Playlist");
                    }
                    if (idClaimConvet != playlist.user)
                    {
                        return BadRequest(400);
                    }


                    _playlistRepository.Delete(playlist);

                }

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