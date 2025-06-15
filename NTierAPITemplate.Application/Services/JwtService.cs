using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NTierAPITemplate.Application.Dtos;
using NTierAPITemplate.Application.Interfaces;
using NTierAPITemplate.Common.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NTierAPITemplate.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;
        public JwtService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateToken(UserDto user)
        {
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
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
