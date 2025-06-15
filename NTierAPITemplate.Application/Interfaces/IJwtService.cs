using NTierAPITemplate.Application.Dtos;
using System.Security.Claims;

namespace NTierAPITemplate.Application.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(string email, string password);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
