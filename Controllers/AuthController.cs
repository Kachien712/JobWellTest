using JobWell.Data;
using JobWell.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobWell.Services;

namespace JobWell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly PasswordHashService passwordHashService;
        public AuthController(ApplicationDbContext applicationDbContext, PasswordHashService passwordHashService)
        {
            this.applicationDbContext = applicationDbContext;
            this.passwordHashService = passwordHashService;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userDTO.Password != userDTO.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match!" });
            }

            var objUser = applicationDbContext.Users.FirstOrDefault(u => u.Email == userDTO.Email);
            if (objUser != null)
            {
                return BadRequest(new { message = "User with this email already exists!" });
            }
            else
            {
                var hashedPassword = passwordHashService.HashPassword(userDTO.Password);
                applicationDbContext.Users.Add(new User
                {
                    FullName = userDTO.FullName,
                    Email = userDTO.Email,
                    PasswordHash = hashedPassword,
                });
                applicationDbContext.SaveChanges();
                return Ok(new { message = "User registered successfully!" });
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var user = applicationDbContext.Users.FirstOrDefault(u => u.Email == loginDTO.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Email or password is incorrect!" });
            }

            bool isPassWordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash);
            if (!isPassWordValid)
            {
                return BadRequest(new { message = "Email or password is incorrect" });
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
    }
}
