using System.Security.Claims;

namespace MinimalWebApi.Services;

public interface IJwtTokenGeneratorService
{
    Task<string?> GenerateToken(string? username, string? password);
    public ClaimsPrincipal? ValidateToken(string token);

}