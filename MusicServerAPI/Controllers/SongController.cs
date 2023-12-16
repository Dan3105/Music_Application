using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
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
        private readonly IArtistRepository _artistRepository;

        public SongController(IArtistRepository artistRepository, ISongRepository songRepository, IUserRepository userRepository)
        {
            _artistRepository = artistRepository;
            _songRepository = songRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSongs()
        {
            ICollection<Song> songs = await _songRepository.GetSongs();
            List<SongDTO> songList = new List<SongDTO>();
            foreach (var song in songs)
            {
                songList.Add(new SongDTO(song));
            }
            return Ok(songList);
        }

        [HttpGet("by-artist-{id}")]
        public async Task<IActionResult> GetSongsByArtist(int id)
        {
            ICollection<Song> songs = await _songRepository.GetSongsByArtistId(id);
            List<SongDTO> songList = new List<SongDTO>();

            foreach (var song in songs)
            {
                songList.Add(new SongDTO(song));
            }

            return Ok(songList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSong(int id)
        {
            try
            {
                var song = await _songRepository.GetSong(id);
                return Ok(new SongDTO(song));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("most-liked")]
        public async Task<IActionResult> GetMostLikeSong()
        {
            ICollection<Song> songs = await _songRepository.GetSongsOrderLikes();
            List<SongDTO> songList = new List<SongDTO>();

            foreach (var song in songs)
            {
                songList.Add(new SongDTO(song));
            }

            return Ok(songList);
        }

        [HttpGet("search/{text}")]
        public async Task<IActionResult> GetMostLikeSong(string text)
        {
            ICollection<Song> songs = await _songRepository.GetSongsBySearch(text);
            List<SongDTO> songList = new List<SongDTO>();

            foreach (var song in songs)
            {
                songList.Add(new SongDTO(song));
            }

            return Ok(songList);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestSong()
        {
            ICollection<Song> songs = await _songRepository.GetSongsOrderDateRealease();
            List<SongDTO> songList = new List<SongDTO>();

            foreach(var song in songs)
            {
                songList.Add(new SongDTO(song));
            }
            return Ok(songList);
        }

        [Authorize(Policy = "CanCustomMusic")]
        [HttpPatch("like/{id}")]
        public async Task<IActionResult> PatchLikeMusic(int id)
        {
            try
            {
                string emailClaim = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (emailClaim == null)
                {
                    return BadRequest(400);
                }
                User user = await _userRepository.GetUser(emailClaim); //_context.Users.FirstOrDefault(x => x.Email == emailClaim);

                if (user.FavoriteSongs.Select(p => p.SongId).Contains(id))
                {
                    Song song = await _songRepository.GetSong(id);
                    song.Likes -= 1;
                    user.FavoriteSongs = user.FavoriteSongs.Where(p => p.SongId != id).ToList();
                    _userRepository.Update(user);
                    _songRepository.Update(song);
                }
                else
                {
                    Song song = await _songRepository.GetSong(id);
                    song.Likes += 1;
                    user.FavoriteSongs.Add(new FavoriteSongs() { 
                        SongId = song.Id
                    });
                    _userRepository.Update(user);
                    _songRepository.Update(song);
                }

                UserRequest userRequest = new UserRequest()
                {
                    Id = user.Id,
                    UserEmail = emailClaim,
                    Roles = user.UserRoles.Select(r => r.Role).Select(role => role.RoleName).ToArray(),
                    Favorites = user.FavoriteSongs.Select(fs=>fs.SongId).ToArray()
                };
                return Ok(userRequest);
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(400);
            }
        }

        [Authorize(Policy = "CanCustomMusic")]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateMusic([FromBody] SongDTO songDTO)
        {
            try
            {
                Song song = await _songRepository.GetSong(songDTO.id);
                song.Title = songDTO.title;
                song.Duration = songDTO.duration;
                song.Likes = songDTO.likes;
                song.CoverImage = songDTO.coverImage;
                song.SongURL = songDTO.songURL;
                song.ReleaseDate = songDTO.releaseDate;

                var existingArtist = song.ArtistSongs.Select(p => p.ArtistId);
                var selectedArtist = songDTO.artists.Select(a => a.id);

                var toRemove = existingArtist.Except(selectedArtist).ToList();
                var toAdd = selectedArtist.Except(existingArtist).ToList();

                song.ArtistSongs = song.ArtistSongs.Where(x => !toRemove.Contains(x.ArtistId)).ToList();
                foreach(var artistId in toAdd)
                {
                    song.ArtistSongs.Add(new ArtistSong()
                    {
                        ArtistId = artistId,
                    });
                }

                _songRepository.Update(song);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [Authorize(Policy = "CanCustomMusic")]
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            try
            {
                _songRepository.Delete(await _songRepository.GetSong(id));
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [Authorize(Policy = "CanCustomMusic")]
        [HttpPost("add")]
        public async Task<IActionResult> AddSong([FromBody] SongDTO song)
        {
            try
            {
                var listArtistContain = (await _artistRepository.GetSubArtists(song.artists.Select(p => p.id))).Select(ar => ar.Id);

                var newSong = new Song();
                newSong.Title = song.title;
                newSong.Duration = song.duration;
                newSong.Likes = song.likes;
                newSong.CoverImage = song.coverImage;
                newSong.SongURL = song.songURL;
                newSong.ReleaseDate = song.releaseDate;

                newSong.ArtistSongs = new List<ArtistSong>();
                foreach(var artistId in listArtistContain)
                {
                    newSong.ArtistSongs.Add(new ArtistSong()
                    {
                        ArtistId = artistId,
                    });
                };


                _songRepository?.CreateSong(newSong);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }
    }
}
