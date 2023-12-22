using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;
using MusicServerAPI.Repository;


namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : Controller
    {
        public IArtistRepository _artistRepository;
        public ISongRepository _songRepository;

        public ArtistController(IArtistRepository artistRepository,
            ISongRepository songRepository)
        {
            _artistRepository = artistRepository;
            _songRepository = songRepository;
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

        [Authorize(Policy= "CanCustomArtist")]
        [HttpPatch]
        public async Task<IActionResult> UpdateArtist([FromBody] ArtisteDTO artistDTO)
        {
            try
            {
                var songsList = await _songRepository.GetSongsByListId(artistDTO.Songs.Select(s => s.id));
                Artist artist = await _artistRepository.GetArtistFetchSong(artistDTO.id);
                //artist.Songs = new HashSet<Song>(songsList);
                artist.Name = artistDTO.name;
                artist.Image = artistDTO.image;
                artist.Biography = artistDTO.bio;
                var existingSong = artist.ArtistSongs != null ? artist.ArtistSongs.Select(p => p.SongId).ToList()
                    : new List<int>();
                var selectedSong = artistDTO.Songs.Select(s => s.id).ToList();

                var toAdd = selectedSong.Except(existingSong).ToList();
                var toRemove = existingSong.Except(selectedSong).ToList();

                artist.ArtistSongs = artist.ArtistSongs.Where(x => !toRemove.Contains(x.SongId)).ToList();
                foreach(var songId in toAdd)
                {
                    artist.ArtistSongs.Add(new ArtistSong()
                    {
                        SongId = songId
                    });
                }
                _artistRepository.Update(artist);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [Authorize(Policy="CanCustomArtist")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            try
            {
                Artist artist = await _artistRepository.GetArtist(id);
                if (artist != null)
                {
                    _artistRepository.Delete(artist);
                }
                return Ok();
            }

            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return BadRequest();
            }
        }

        [Authorize(Policy = "CanCustomArtist")]
        [HttpPost]
        public async Task<IActionResult> CreateArtist([FromBody] ArtisteDTO artisteDTO)
        {
            try
            {
                var songsFromArtist = (await _songRepository.GetSongsByListId(artisteDTO.Songs.Select(p => p.id))).Select(sr => sr.Id);
                Artist artist = new Artist();
                artist.Biography = artisteDTO.bio;
                artist.Name = artisteDTO.name;
                artist.Image = artisteDTO.image;
                artist.ArtistSongs = new List<ArtistSong>();
                foreach(var songId in songsFromArtist)
                {
                    artist.ArtistSongs.Add(new ArtistSong()
                    {
                        SongId = songId
                    });
                }
                _artistRepository.CreateArtist(artist);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();
            }
        }
    }
}
