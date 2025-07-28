using JobWellTest.DTOs.AccountManagement;
using JobWellTest.DTOs.Authentication;
using JobWellTest.DTOs.Authentication;
using JobWellTest.DTOs.UserManagement;
using JobWellTest.Extensions;
using JobWellTest.Interfaces;
using JobWellTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Numerics;

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
                    TwoFactorEnabled = false,
                };

                var createUser = await _userManager.CreateAsync(user, registerDTO.Password);

                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmationLink = Url.Action(
                            nameof(ConfirmEmail),
                            "Account",
                            new { email = user.Email, token = token },
                            Request.Scheme
                        );

                        var emailBody = $"<h1>Welcome to JobWell!</h1>" +
                         $"<p>Please confirm your account by clicking the link below:</p>" +
                         $"<a href='{confirmationLink}'>Click here to confirm</a>";

                        await _emailService.SendEmail(user.Email, "Confirm Your Email", emailBody);
                        return Ok(new { message = "Registration successful. A confirmation link has been sent to your email." });
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
                return Unauthorized("UserName not found or password is incorrect.");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action(
                    nameof(ConfirmEmail),
                    "Account",
                    new { email = user.Email, token = token },
                    Request.Scheme
                );

                var emailBody = $"<h1>Welcome to JobWell!</h1>" +
                 $"<p>Please confirm your account by clicking the link below:</p>" +
                 $"<a href='{confirmationLink}'>Click here to confirm</a>";

                await _emailService.SendEmail(user.Email, "Confirm Your Email", emailBody);
                return Ok(new { message = "Email is not confirmed. A confirmation link has been sent to your email." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("UserName not found or password is incorrect.");
            }

            if (user.TwoFactorEnabled)
            {
                var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                var emailBody = $"<h1>Two Factor Authentication Code</h1>" +
                    $"<p>This is your code: {code}</p>";

                await _emailService.SendEmail(user.Email, "Two Factor Authentication Code", emailBody);

                return Ok(new
                {
                    TwoFactorRequired = true,
                    Email = user.Email,
                    message = "Your two factor authentication code has been sent to your email."
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

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid confirmation link parameters.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Cannot found user with this email.");
            }

            var isVerified = await _userManager.ConfirmEmailAsync(user, token);
            if (isVerified.Succeeded)
            {
                return Content("<h1>Email confirmed successfully! You can now close this tab and log in.</h1>", "text/html");
            }
            return BadRequest("Could not confirm email. The link may have expired.");
        }

        [HttpPost("login-2fa")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginTwoFactor([FromBody] Login2faDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized("Cannot find an user with this email.");
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", dto.Code);

            if (isValid)
            {
                return Ok(
                    new LoginResponseDTO
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    }
                );
            }
            return Unauthorized("Invalid authentication code.");
        }

        [HttpPost("resend-confirmation-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Ok(new { message = "Cannot found an user with this email." });
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new { message = "This account has already been confirmed." });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action(
                nameof(ConfirmEmail),
                "Account",
                new { email = user.Email, token = token },
                Request.Scheme
            );

            var emailBody = $"<h1>Welcome to JobWell!</h1>" +
             $"<p>Please confirm your account by clicking the link below:</p>" +
             $"<a href='{confirmationLink}'>Click here to confirm</a>";

            await _emailService.SendEmail(user.Email, "Confirm Your Email", emailBody);

            return Ok(new { message = "A confirmation link has been sent to your email. Please check your email." });
        }

        [HttpPost("resend-2fa-code")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendTwoFactorAuthenticationCode (Resend2faCodeDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Ok(new { message = "Cannot found an user with this email." });
            }

            if (!user.TwoFactorEnabled)
            {
                return BadRequest("Two-factor authentication is not enabled for this account.");
            }

            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            var emailBody = $"<h1>Two Factor Authentication Code</h1>" +
                $"<p>This is your code: {code}</p>";

            await _emailService.SendEmail(user.Email, "Two Factor Authentication Code", emailBody);

            return Ok(new
            {
                TwoFactorRequired = true,
                Email = user.Email,
                message = "Your two factor authentication code has been sent to your email."
            });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByNameAsync(User.GetUsername());

            var result = await _userManager.ChangePasswordAsync(user, dto.Password, dto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Password successfully changed." });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Unauthorized("User with this email does not exist.");
            }

            var token = _userManager.GeneratePasswordResetTokenAsync(user);

            var emailBody = $"<h1>Password Reset Verification</h1>" +
                $"<p>This is your verification code: {token}</p>";

            await _emailService.SendEmail(dto.Email, "Password reset verification.", emailBody);

            return Ok(new { message = "Your password verification code has been sent to your email." });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "User with this email is not exist." });
            }

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Password has been reset successfully" });
        }
    }
}
