using System.Security.Claims;

namespace MboaPrice.Api.Endpoints;

public static class ApiExtensions
{
    /// <summary>Identifiant de l'utilisateur lu dans le jeton JWT (claim « sub »), ou null si anonyme.</summary>
    public static int? GetUserId(this ClaimsPrincipal user) =>
        int.TryParse(user.FindFirstValue("sub"), out var id) ? id : null;

    public static IResult Fail(string message, int status = 400) =>
        Results.Json(new ApiError(message), statusCode: status);
}
