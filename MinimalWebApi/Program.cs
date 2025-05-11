using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using MinimalWebApi.Configurations;
using MinimalWebApi.Repositories;
using MinimalWebApi.Services;
using MinimalWebApi.Validators;

namespace MinimalWebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var jwtConfig = builder.Configuration
            .GetSection("Authentication")
            .Get<JwtConfiguration>();
        
        ArgumentNullException.ThrowIfNull(jwtConfig);

        builder.Services.AddSingleton(jwtConfig);
        
        builder.Services.AddControllers();

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
        
        builder.Services.AddOpenApi();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();

        
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Convert.FromBase64String(jwtConfig.SecretKey)
                        )
                    };
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        
        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}