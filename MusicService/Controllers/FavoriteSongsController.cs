using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Repository;

namespace MusicService.Controllers
{
    [Route("api/MusicService/[controller]")]
    [Authorize]
    [ApiController]
    public class FavoriteSongsController : Controller
    {
        private readonly IFavoriteRepository _favoriteRepository;
        public FavoriteSongsController(IFavoriteRepository favoriteRepository) { 
            _favoriteRepository = favoriteRepository;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetFavoriteSong(int id)
        {
            var listId = await _favoriteRepository.GetFavoritesByUserId(id);
            return Ok(listId);
        }
    }
}
