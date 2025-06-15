using NTierAPITemplate.Application.Dtos;
using System.Security.Claims;

namespace NTierAPITemplate.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserDto user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
