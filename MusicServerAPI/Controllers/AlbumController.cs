using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;
using MusicServerAPI.Repository;

namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : Controller
    {
        public ISongRepository songRepository;
        public IAlbumRepository albumRepository;
        public IArtistRepository artistRepository;

        public AlbumController(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository)
        {
            this.songRepository = songRepository;
            this.albumRepository = albumRepository;
            this.artistRepository = artistRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbum()
        {
            ICollection<Album> albums = await albumRepository.GetAlbums();
            List<AlbumDTO> albumList = new List<AlbumDTO>();
            foreach (var album in albums)
            {
                albumList.Add(new AlbumDTO(album));
            }
            return Ok(albumList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtist(int id)
        {
            Album album = await albumRepository.GetAlbum(id);
            return Ok(new AlbumDTO(album));
        }

        [Authorize(Policy = "CanCustomSong")]
        [HttpPatch]
        public async Task<IActionResult> UpdateAlbum([FromBody] AlbumDTO albumDTO)
        {
            try
            {
                var songsList = await songRepository.GetSongsByListId(albumDTO.Songs.Select(s => s.id));
                Artist artist = await artistRepository.GetArtist(albumDTO.Artiste.id);
                Album album = await albumRepository.GetAlbum(albumDTO.Id);
                //artist.Songs = new HashSet<Song>(songsList);
                album.Name = albumDTO.Name;
                album.ImageURL = albumDTO.ImageUrl;
                album.ReleaseDate = albumDTO.ReleaseDate;
                album.Artist = artist;
                var existingSong = album.Songs != null ? album.Songs.Select(p => p.Id).ToList()
                    : new List<int>();
                var selectedSong = albumDTO.Songs.Select(s => s.id).ToList();

                var toAdd = selectedSong.Except(existingSong).ToList();
                var toRemove = existingSong.Except(selectedSong).ToList();

                album.Songs = album.Songs.Where(x => !toRemove.Contains(x.Id)).ToList();
                foreach (var songId in toAdd)
                {
                    album.Songs.Add(new Song()
                    {
                        Id = songId
                    });
                }
                albumRepository.Update(album);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [Authorize(Policy = "CanCustomSong")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            try
            {
                Album album = await albumRepository.GetAlbum(id);
                if (album != null)
                {
                    albumRepository.Delete(album);
                }
                return Ok();
            }

            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return BadRequest();
            }
        }

        [Authorize(Policy = "CanCustomSong")]
        [HttpPost]
        public async Task<IActionResult> CreateArtist([FromBody] AlbumDTO albumDTO)
        {
            try
            {
                var songsFromAlbum = (await songRepository.GetSongsByListId(albumDTO.Songs.Select(p => p.id))).ToList();
                var artist = await artistRepository.GetArtist(albumDTO.Artiste.id);
                Album album = new Album();
                album.Name = albumDTO.Name;
                album.ImageURL = albumDTO.ImageUrl;
                album.ReleaseDate = albumDTO.ReleaseDate;
                album.Artist = artist;
                album.Songs = songsFromAlbum;
                
                albumRepository.CreateAlbum(album);

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
