using Microsoft.AspNetCore.Mvc;
using MusicServerAPI.Data;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;
using MusicServerAPI.Repository;
using System.Collections;

namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : Controller
    {
        public MusicServerAPIContext _dbContext;
        public IArtistRepository _artistRepository;

        public ArtistController(MusicServerAPIContext dbContext, IArtistRepository artistRepository)
        {
            _dbContext = dbContext;
            _artistRepository = artistRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            ICollection<Artist> artistes = await _artistRepository.GetArtists();
            List<ArtisteDTO> artisteList = new List<ArtisteDTO>();
            foreach(var artist in artistes)
            {
                artisteList.Add(new ArtisteDTO(artist));
            }
            return Ok(artisteList);
        }

        [HttpGet("with-songs/{id}")]
        public async Task<IActionResult> GetArtistWithSongs(int id)
        {
            try
            {
                Artist artist = await _artistRepository.GetArtistFetchSong(id);
                ArtisteDTO artisteDTO = new ArtisteDTO(artist);
                return Ok(artisteDTO);
            }
            catch
            {
                return BadRequest("Failed in Request");
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtist(int id)
        {
            Artist artist = await _artistRepository.GetArtist(id);
            return Ok(new ArtisteDTO(artist));
        }
    }
}
