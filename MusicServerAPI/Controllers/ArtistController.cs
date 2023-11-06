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
        public ICollection<ArtisteDTO> GetArtists()
        {
            ICollection<Artist> artistes = _artistRepository.GetArtists();
            List<ArtisteDTO> artisteList = new List<ArtisteDTO>();
            foreach(var artist in artistes)
            {
                artisteList.Add(new ArtisteDTO(artist));
            }
            return artisteList;
        }

        [HttpGet("{id}")]
        public ArtisteDTO GetArtist(int id)
        {
            Artist artist = _artistRepository.GetArtist(id);
            return new ArtisteDTO(artist);
        }
    }
}
