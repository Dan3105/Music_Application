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
        private readonly IRoleRepository _roleRepository;
        public UserController(IUserRepository userRepository, IRoleRepository roleRepository) { 
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers() {
            List<User> users = (await _userRepository.GetAll()).ToList();
            List<UsersDTO> usersDTOs  = new List<UsersDTO>();
            foreach (var user in users)
            {
                usersDTOs.Add(new UsersDTO(user));
            }
            return Ok(usersDTOs);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> PatchUsers([FromBody] UsersDTO aUser)
        {
            User userFromDB = await _userRepository.GetUser(aUser.Id);
            if (userFromDB == null)
            {
                return BadRequest();
            }
            userFromDB.Is_activate = aUser.IsActive;
            var listIdRoles = aUser.roleDTOs.Select(p => p.Id);
            var roles = await _roleRepository.GetSubRoles(listIdRoles);

            userFromDB.Roles = roles;
            _userRepository.Update(userFromDB);
            _userRepository.SaveChanges();
            return Ok("Update User Successfully");
        }
    }
}
