using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicService.Entity;
using MusicService.Model;
using MusicService.Repository;
using System.Security.Claims;

namespace MusicService.Controllers
{
    [Route("api/MusicService/[controller]")]
    [ApiController]
    public class SongController : Controller
    {
        private readonly ISongRepository _songRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IArtistRepository _artistRepository;

        public SongController(IArtistRepository artistRepository, ISongRepository songRepository, IFavoriteRepository favoriteRepository)
        {
            _artistRepository = artistRepository;
            _songRepository = songRepository;
            _favoriteRepository = favoriteRepository;
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

        [Authorize]
        [HttpPatch("like/{id}")]
        public async Task<IActionResult> PatchLikeMusic(int id)
        {
            try
            {
                string idClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (idClaim == null)
                {
                    return BadRequest(400);
                }

                //User user = await _userRepository.GetUser(emailClaim); //_context.Users.FirstOrDefault(x => x.Email == emailClaim);
                if(int.TryParse(idClaim, out int idClaimConvert))
                {
                    var listFavorites = await _favoriteRepository.GetFavoritesByUserId(idClaimConvert);
                    if (listFavorites.FirstOrDefault(s => s.Id == id) != null)
                    {
                        Song song = await _songRepository.GetSong(id);
                        song.Likes -= 1;
                        //user.FavoriteSongs = user.FavoriteSongs.Where(p => p.SongId != id).ToList();
                        await _favoriteRepository.Delete(idClaimConvert, song.Id);
                        _songRepository.Update(song);
                    }
                    else
                    {
                        Song song = await _songRepository.GetSong(id);
                        song.Likes += 1;
                        //_userRepository.Update(user);
                        await _favoriteRepository.Add(idClaimConvert, song.Id);
                        _songRepository.Update(song);
                    }
                }
                else
                {
                    return BadRequest(400);
                }
                
                return Ok();
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
