using System.Text.RegularExpressions;
using MboaPrice.Api.Data;
using MboaPrice.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MboaPrice.Api.Endpoints;

public static partial class AuthEndpoints
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    public static void MapAuth(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/auth").WithTags("Authentification");

        auth.MapPost("/register", async (RegisterRequest req, AppDbContext db, IPasswordHasher<User> hasher, TokenService tokens) =>
        {
            var name = req.FullName?.Trim() ?? "";
            var email = req.Email?.Trim().ToLowerInvariant() ?? "";
            var password = req.Password ?? "";

            if (name.Length < 2) return ApiExtensions.Fail("Indiquez votre nom complet.");
            if (!EmailRegex().IsMatch(email)) return ApiExtensions.Fail("Adresse e-mail invalide.");
            if (password.Length < 8 || !password.Any(char.IsDigit))
                return ApiExtensions.Fail("Le mot de passe doit contenir 8 caractères minimum, dont un chiffre.");
            if (await db.Users.AnyAsync(u => u.Email == email))
                return ApiExtensions.Fail("Un compte existe déjà avec cette adresse e-mail.", StatusCodes.Status409Conflict);

            var user = new User { FullName = name, Email = email };
            user.PasswordHash = hasher.HashPassword(user, password);
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Ok(new AuthResponse(tokens.Create(user), new UserDto(user.Id, user.FullName, user.Email)));
        });

        auth.MapPost("/login", async (LoginRequest req, AppDbContext db, IPasswordHasher<User> hasher, TokenService tokens) =>
        {
            var email = req.Email?.Trim().ToLowerInvariant() ?? "";
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            var ok = user is not null &&
                     hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password ?? "") != PasswordVerificationResult.Failed;

            return ok
                ? Results.Ok(new AuthResponse(tokens.Create(user!), new UserDto(user!.Id, user.FullName, user.Email)))
                : ApiExtensions.Fail("Adresse e-mail ou mot de passe incorrect.", StatusCodes.Status401Unauthorized);
        });

        app.MapGet("/api/me", async (HttpContext http, AppDbContext db) =>
        {
            var id = http.User.GetUserId();
            var user = id is null ? null : await db.Users.FindAsync(id);
            return user is null
                ? Results.Unauthorized()
                : Results.Ok(new UserDto(user.Id, user.FullName, user.Email));
        }).RequireAuthorization().WithTags("Authentification");
    }
}
