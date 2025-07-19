using JobWell.Data;
using JobWell.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobWell.Services;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;


namespace JobWell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly PasswordHashService passwordHashService;
        private readonly EmailService emailService;
        public AuthController(ApplicationDbContext applicationDbContext, PasswordHashService passwordHashService, EmailService emailService)
        {
            this.applicationDbContext = applicationDbContext;
            this.passwordHashService = passwordHashService;
            this.emailService = emailService;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var objUser = applicationDbContext.Users.FirstOrDefault(u => u.Email == registerDTO.Email);
            if (objUser != null)
            {
                return BadRequest(new { message = "User with this email already exists!" });
            }
            else
            {
                var hashedPassword = passwordHashService.HashPassword(registerDTO.Password);
                var user = new User
                {
                    FullName = registerDTO.FullName,
                    Email = registerDTO.Email,
                    PasswordHash = hashedPassword,
                };
                applicationDbContext.Users.Add(user);
                // Generate a random activation code
                var activationCode = new Random().Next(100000, 999999).ToString();
                emailService.SendEmail(
                    user.Email,
                    "Account Activation",
                    $"<h1>Welcome {user.FullName}!</h1><p>Your activation code is {activationCode}.</p>"
                );
                user.ActivationCode = activationCode;
                applicationDbContext.SaveChanges();
                return Ok(new { message = "User registered successfully!" });
            }
        }

        [HttpPost]
        [Route("ActivateAccount")]
        public IActionResult ActivateAccount(ActivateAccountDTO dto)
        {
            var user = applicationDbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
            {
                return BadRequest(new { message = "User not found!" });
            }
            user.isActive = 1;
            user.ActivationCode = null;
            applicationDbContext.SaveChanges();
            return Ok(new { message = "Account activated successfully!" });
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
