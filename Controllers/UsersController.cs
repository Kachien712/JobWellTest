using JobWell.Data;
using JobWell.DTO;
using JobWell.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobWell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        public UsersController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var objUser = applicationDbContext.Users.FirstOrDefault(u => u.Email == userDTO.Email);
            if (objUser != null)
            {
                return BadRequest(new { message = "User with this email already exists!" });
            } else
            {
                applicationDbContext.Users.Add(new User
                {
                    FullName = userDTO.FullName,
                    Email = userDTO.Email,
                    Password = userDTO.Password
                });
                applicationDbContext.SaveChanges();
                return Ok(new { message = "User registered successfully!" });
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login (LoginDTO loginDTO)
        {
            var user = applicationDbContext.Users.FirstOrDefault(u => u.Email == loginDTO.Email && u.Password == loginDTO.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Email or password is incorrect!" });
            }
            else
            {
                var userResponse = new LoginResponseDTO
                {
                    FullName = user.FullName,
                    Email = user.Email
                };
                return Ok(userResponse);
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = applicationDbContext.Users.ToList();
            if (users == null || !users.Any())
            {
                return NoContent();
            }
            else
            {
                return Ok(users);
            }
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser (int id)
        {
            var user = applicationDbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(user);
            }
        }
    }
}
