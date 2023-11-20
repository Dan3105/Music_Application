using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicServerAPI.Entity;
using MusicServerAPI.Model;
using MusicServerAPI.Repository;

namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) { 
            _userRepository = userRepository;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetUsers() {
            List<User> users = (await _userRepository.GetAll()).ToList();
            List<UsersDTO> usersDTOs  = new List<UsersDTO>();
            foreach (var user in users)
            {
                usersDTOs.Add(new UsersDTO(user));
            }
            return Ok(usersDTOs);
        }
    }
}
