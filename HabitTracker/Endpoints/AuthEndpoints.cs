using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using HabitTracker.Entities;

namespace HabitTracker.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (
            [FromBody] RegisterDto dto,
            UserManager<AppUser> userManager,
            IConfiguration config) =>
        {
            var email = dto.Email?.Trim() ?? "";
            var password = dto.Password?.Trim() ?? "";

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return Results.BadRequest(new { message = "Пользователь с таким email уже существует" });

            var user = new AppUser
            {
                UserName = email,
                Email = email,
                RegisteredAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return Results.BadRequest(new { errors = result.Errors });

            var token = GenerateJwtToken(user, config);

            return Results.Ok(new
            {
                token,
                tokenType = "Bearer",
                userId = user.Id,
                email = user.Email,
                userName = user.UserName,
                message = "Пользователь успешно зарегистрирован"
            });
        })
        .WithName("Register");

        app.MapPost("/api/auth/login", async (
            [FromBody] LoginDto dto,
            UserManager<AppUser> userManager,
            IConfiguration config) =>
        {
            var email = dto.Email?.Trim() ?? "";
            var password = dto.Password?.Trim() ?? "";

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Results.Unauthorized();

            var isValidPassword = await userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
                return Results.Unauthorized();

            user.LastLoginAt = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            var token = GenerateJwtToken(user, config);

            return Results.Ok(new
            {
                token,
                tokenType = "Bearer",
                expiresIn = int.Parse(config["JwtSettings:ExpiresInMinutes"] ?? "60"),
                userId = user.Id,
                email = user.Email,
                userName = user.UserName
            });
        })
        .WithName("Login");

        app.MapGet("/api/auth/me", async (
            ClaimsPrincipal user,
            UserManager<AppUser> userManager) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var appUser = await userManager.FindByIdAsync(userId);
            if (appUser == null)
                return Results.NotFound();

            return Results.Ok(new
            {
                appUser.Id,
                appUser.Email,
                appUser.UserName,
                appUser.RegisteredAt,
                appUser.LastLoginAt
            });
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser");
    }

    private static string GenerateJwtToken(AppUser user, IConfiguration config)
    {
        var secretKey = config["JwtSettings:SecretKey"];

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new Exception("JwtSettings:SecretKey не найден в конфигурации!");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(
            int.Parse(config["JwtSettings:ExpiresInMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}