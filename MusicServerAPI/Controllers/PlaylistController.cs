using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicServerAPI.Data;
using MusicServerAPI.Model;
using MusicServerAPI.Repository;
using System.Security.Claims;

namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : Controller
    {
        private readonly MusicServerAPIContext _context;
        private readonly IPlaylistRepository _playlistRepository;
        
        public PlaylistController(MusicServerAPIContext context, IPlaylistRepository playlistRepository)
        {
            _context = context;
            _playlistRepository = playlistRepository;
        }

        [HttpGet("user/{id}")]
        public async Task<ICollection<PlaylistDTO>> GetPlaylistsByUserId(int id)
        {
            var playlists = await _playlistRepository.GetPlaylists(id);
            ICollection<PlaylistDTO> playlistDTOs = new List<PlaylistDTO>();
            foreach(var playlist in playlists)
            {
                playlistDTOs.Add(new PlaylistDTO(playlist));
            }

            return playlistDTOs;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<PlaylistDTO> GetPlaylist(int id)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            string email = "";
            if (emailClaim != null)
            {
                email = emailClaim.Value;
            }

            if(email.Length == 0)
            {
                return BadRequest("Fail Authorize");
            }

                var currentUser = _context.Users.FirstOrDefault(x => x.Email == ) 
            var playlist = await _playlistRepository.GetPlaylist(id);
            return new PlaylistDTO(playlist);   
        }
    }
}
