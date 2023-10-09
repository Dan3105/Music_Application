using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly MusicAPIContext _context;

        public AuthController(MusicAPIContext context)
        {
            _context = context;
        }
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<User>> PostUser(RegisterModel registerForm)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'MusicAPIContext.User'  is null.");
            }

            // Validate the user input.
            if (!ModelState.IsValid)
            {
                // Return a BadRequestObjectResult with the validation errors.
                return BadRequest(ModelState);
            }

            // Check if the user already exists in the database.
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == registerForm.Email);
            if (existingUser != null)
            {
                // The user already exists. Return a BadRequestObjectResult with an error message.
                return BadRequest("The user already exists.");
            }

            User new_user = new User();
            new_user.Email = registerForm.Email;
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(registerForm.Password);
            new_user.password = hashPassword;
            try
            {
                _context.User.Add(new_user);
            }
            catch (Exception e)
            {
                if (Utlity.Utility.IsInDebugMode())
                    return BadRequest(e.Message);
                return BadRequest("Error");
            }
            await _context.SaveChangesAsync();

            return Ok("Successfully create user!");
        }

        //POST: login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(RegisterModel request)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'MusicAPIContext.User'  is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(request.Password, user.password))
                {
                    Response.Cookies.Append("user", user.Public_id.ToString());
                    return Ok(user);
                }
            }

            return BadRequest("Incorrect password or email hasn't been registed");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("user");
            return Ok();
        }
    }
}
