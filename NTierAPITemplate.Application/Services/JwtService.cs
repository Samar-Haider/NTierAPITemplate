using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NTierAPITemplate.Application.Dtos;
using NTierAPITemplate.Application.Interfaces;
using NTierAPITemplate.Common.Auth;
using NTierAPITemplate.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NTierAPITemplate.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;
        private readonly UserManager<UserAccount> _users;
        public JwtService(IOptions<JwtSettings> opts, UserManager<UserAccount> users)
        {
            _settings = opts.Value;
            _users = users;
        }

        public async Task<string> GenerateTokenAsync(string email, string password)
        {
            var user = await _users.FindByEmailAsync(email);
            if (user is null || !await _users.CheckPasswordAsync(user, password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            var roles = await _users.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public ClaimsPrincipal? ValidateToken(string token)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
                ValidateLifetime = true
            };
            try
            {
                return new JwtSecurityTokenHandler().ValidateToken(token, parameters, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}
