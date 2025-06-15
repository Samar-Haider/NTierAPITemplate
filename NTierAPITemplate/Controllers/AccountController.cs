using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NTierAPITemplate.Application.Dtos;
using NTierAPITemplate.Application.Interfaces;
using NTierAPITemplate.Common.Auth;
using NTierAPITemplate.Domain.Entities;

namespace NTierAPITemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public AccountController(
            UserManager<UserAccount> userManager,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return BadRequest("Passwords do not match.");

            var user = UserAccount.Create(
                dto.FirstName, dto.LastName,
                dto.Email, dto.UserName,
                dto.ZipCode, dto.ReferralCode);

            user.PhoneNumber = dto.PhoneNumber;

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Registration successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            // try to build the token, will throw if invalid credentials
            string token;
            try
            {
                token = await _jwtService.GenerateTokenAsync(dto.Email, dto.Password);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password.");
            }

            // compute expiry the same way your service does
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            return Ok(new LoginResponse(token, expires));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: email this token to user.Email via your mail service
            return Ok(new { Token = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                return BadRequest("Passwords do not match.");

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound();

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Password has been reset." });
        }
    }
}
