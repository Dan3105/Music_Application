using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicServerAPI.Data;
using MusicServerAPI.Model;
using MusicServerAPI.Repository;

namespace MusicServerAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            List<RoleDTO> roleDTO = new List<RoleDTO>();
            var roles = await _roleRepository.GetAll();
            foreach (var role in roles)
            {
                roleDTO.Add(new RoleDTO(role));
            }
            return Ok(roleDTO);
        }
    }
}
