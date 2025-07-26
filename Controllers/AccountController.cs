using JobWellTest.DTOs.Authentication;
using JobWellTest.Interfaces;
using JobWellTest.Models;
using JobWellTest.DTOs.Authentication;
using JobWellTest.DTOs.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace JobWellTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    UserName = registerDTO.UserName,
                    FullName = registerDTO.FullName,
                    DateOfBirth = registerDTO.DateOfBirth,
                    Email = registerDTO.Email,
                    PhoneNumber = registerDTO.PhoneNumber,
                    CitizenIdentityNumber = registerDTO.CitizenIdentityNumber,
                };

                var createUser = await _userManager.CreateAsync(user, registerDTO.Password);

                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        await _emailService.SendEmail(user.Email, "Please use this code to confirm your email.", $"This is the code: {code}");

                        return Ok(new { message = $"Please confirm your email with the code sent to your email." });

                        //return Ok(
                        //    new RegisterResponseDTO
                        //    {
                        //        UserName = user.UserName,
                        //        Email = user.Email,
                        //        Token = _tokenService.CreateToken(user)
                        //    });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginDTO.UserName.ToLower());
            if (user == null)
            {
                return Unauthorized("Invalid UserName.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("UserName not found or password is incorrect.");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Email is not confirmed",
                    }
                });
            }

            return Ok(
                new LoginResponseDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpPost("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string? email, string? code)
        {
            if (email == null || code == null)
            {
                return BadRequest("Invalid payload.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid payload.");
            }

            var isVerified = await _userManager.ConfirmEmailAsync(user, code);
            if (isVerified.Succeeded)
            {
                return Ok(new
                {
                    message = "Email confirmed."
                });
            }
            return BadRequest("something went wrong.");
        }

        //[HttpPut]
        //public async Task<IActionResult> ChangePassword()
        //{

        //}
    }
}
