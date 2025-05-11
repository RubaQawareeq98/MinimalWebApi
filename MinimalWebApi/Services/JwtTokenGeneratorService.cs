using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MinimalWebApi.Configurations;
using MinimalWebApi.Repositories;

namespace MinimalWebApi.Services;

public class JwtTokenGeneratorService(JwtConfiguration configuration, IUserRepository userRepository) : IJwtTokenGeneratorService
{
    public async Task<string?> GenerateToken(string? username, string? password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return null;
        }
        var user = await userRepository.GetUserAsync(username, password);
        if (user is null)
        {
            return null;
        }
        
        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(configuration.SecretKey));
            
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claimsForToken = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("firstName", user.FirstName),
            new("email", user.Email),
        };

        var jwt = new JwtSecurityToken(
            configuration.Issuer,
            configuration.Audience,
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(jwt);
            
        return token;
    }
    
    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration.SecretKey);

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration.Issuer,
            ValidAudience = configuration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            return tokenHandler.ValidateToken(token, parameters, out _);
        }
        catch
        {
            return null;
        }
    }
}
