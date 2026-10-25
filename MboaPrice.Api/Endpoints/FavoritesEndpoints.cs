using MboaPrice.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MboaPrice.Api.Endpoints;

public static class FavoritesEndpoints
{
    public static void MapFavorites(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/me/favorites").RequireAuthorization().WithTags("Favoris");

        g.MapGet("/", async (HttpContext http, AppDbContext db) =>
        {
            var uid = http.User.GetUserId()!.Value;
            return Results.Ok(new
            {
                commodities = await db.FavoriteCommodities.Where(f => f.UserId == uid).Select(f => f.CommodityId).ToListAsync(),
                markets = await db.FavoriteMarkets.Where(f => f.UserId == uid).Select(f => f.MarketId).ToListAsync()
            });
        });

        g.MapPut("/commodities/{id}", async (string id, HttpContext http, AppDbContext db) =>
        {
            var uid = http.User.GetUserId()!.Value;
            if (!await db.Commodities.AnyAsync(c => c.Id == id)) return ApiExtensions.Fail("Denrée introuvable.", 404);
            if (!await db.FavoriteCommodities.AnyAsync(f => f.UserId == uid && f.CommodityId == id))
            {
                db.FavoriteCommodities.Add(new FavoriteCommodity { UserId = uid, CommodityId = id });
                await db.SaveChangesAsync();
            }
            return Results.NoContent();
        });

        g.MapDelete("/commodities/{id}", async (string id, HttpContext http, AppDbContext db) =>
        {
            var uid = http.User.GetUserId()!.Value;
            await db.FavoriteCommodities.Where(f => f.UserId == uid && f.CommodityId == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });

        g.MapPut("/markets/{id}", async (string id, HttpContext http, AppDbContext db) =>
        {
            var uid = http.User.GetUserId()!.Value;
            if (!await db.Markets.AnyAsync(m => m.Id == id)) return ApiExtensions.Fail("Marché introuvable.", 404);
            if (!await db.FavoriteMarkets.AnyAsync(f => f.UserId == uid && f.MarketId == id))
            {
                db.FavoriteMarkets.Add(new FavoriteMarket { UserId = uid, MarketId = id });
                await db.SaveChangesAsync();
            }
            return Results.NoContent();
        });

        g.MapDelete("/markets/{id}", async (string id, HttpContext http, AppDbContext db) =>
        {
            var uid = http.User.GetUserId()!.Value;
            await db.FavoriteMarkets.Where(f => f.UserId == uid && f.MarketId == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}
