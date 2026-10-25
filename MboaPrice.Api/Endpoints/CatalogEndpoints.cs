using MboaPrice.Api.Services;

namespace MboaPrice.Api.Endpoints;

/// <summary>Lecture des prix : accessible sans compte (les favoris ne sont renvoyés que si un jeton est fourni).</summary>
public static class CatalogEndpoints
{
    public static void MapCatalog(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api").WithTags("Prix");

        // Tout l'écran d'accueil / catalogue / détail / comparaison / tendances en un seul appel (utilisé par l'app).
        g.MapGet("/overview", async (HttpContext http, OverviewService svc, CancellationToken ct) =>
            Results.Ok(await svc.BuildAsync(http.User.GetUserId(), ct)));

        g.MapGet("/commodities", async (string? category, string? search, HttpContext http, OverviewService svc, CancellationToken ct) =>
        {
            var overview = await svc.BuildAsync(http.User.GetUserId(), ct);
            var items = overview.Commodities.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(category))
                items = items.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(search))
                items = items.Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            return Results.Ok(items.ToList());
        });

        g.MapGet("/commodities/{id}", async (string id, HttpContext http, OverviewService svc, CancellationToken ct) =>
        {
            var overview = await svc.BuildAsync(http.User.GetUserId(), ct);
            var item = overview.Commodities.FirstOrDefault(c => c.Id == id);
            return item is null ? ApiExtensions.Fail("Denrée introuvable.", 404) : Results.Ok(item);
        });

        g.MapGet("/commodities/{id}/history", async (string id, int? days, HttpContext http, OverviewService svc, CancellationToken ct) =>
        {
            var overview = await svc.BuildAsync(http.User.GetUserId(), ct);
            var item = overview.Commodities.FirstOrDefault(c => c.Id == id);
            if (item is null) return ApiExtensions.Fail("Denrée introuvable.", 404);
            var take = Math.Clamp(days ?? 7, 1, 30) + 1;
            return Results.Ok(item.History.Skip(item.History.Count - take).ToList());
        });

        g.MapGet("/markets", async (HttpContext http, OverviewService svc, CancellationToken ct) =>
            Results.Ok((await svc.BuildAsync(http.User.GetUserId(), ct)).Markets));
    }
}
