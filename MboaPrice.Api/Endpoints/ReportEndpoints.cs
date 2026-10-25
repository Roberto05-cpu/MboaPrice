using MboaPrice.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MboaPrice.Api.Endpoints;

public static class ReportEndpoints
{
    private static readonly string[] Units = { "kg", "L", "sac", "tas", "pièce" };
    private const int MaxReportsPerDay = 20;

    public static void MapReports(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api").RequireAuthorization().WithTags("Relevés");

        g.MapPost("/reports", async (CreateReportRequest req, HttpContext http, AppDbContext db) =>
        {
            var uid = http.User.GetUserId()!.Value;
            var today = Clock.Today;

            if (!await db.Commodities.AnyAsync(c => c.Id == req.CommodityId)) return ApiExtensions.Fail("Denrée inconnue.");
            if (!await db.Markets.AnyAsync(m => m.Id == req.MarketId)) return ApiExtensions.Fail("Marché inconnu.");
            if (req.Price is < 1 or > 1_000_000) return ApiExtensions.Fail("Prix invalide.");
            if (!Units.Contains(req.Unit)) return ApiExtensions.Fail("Unité invalide.");

            var day = req.ObservedAt.Date;
            if (day > today) return ApiExtensions.Fail("La date d'observation ne peut pas être dans le futur.");
            if (day < today.AddDays(-30)) return ApiExtensions.Fail("Le relevé est trop ancien (30 jours maximum).");

            // Limite quotidienne par utilisateur (anti-spam).
            var since = DateTime.UtcNow.AddHours(-24);
            if (await db.Reports.CountAsync(r => r.UserId == uid && r.CreatedAt >= since) >= MaxReportsPerDay)
                return ApiExtensions.Fail("Limite de 20 relevés par jour atteinte.", StatusCodes.Status429TooManyRequests);

            // Prix aberrant : plus de 5 fois au-dessus ou au-dessous de la moyenne des 14 derniers jours.
            var avg = await db.Reports
                .Where(r => r.CommodityId == req.CommodityId && r.ObservedAt >= today.AddDays(-14))
                .AverageAsync(r => (double?)r.Price);
            if (avg is double a && (req.Price > a * 5 || req.Price < a / 5))
                return ApiExtensions.Fail("Ce prix semble incohérent par rapport aux relevés récents. Vérifiez l'unité et la quantité.");

            var note = (req.Note ?? "").Trim();
            if (note.Length > 200) note = note[..200];

            var observed = day == today ? DateTime.UtcNow.AddHours(1) : day.AddHours(12);
            var report = new PriceReport
            {
                CommodityId = req.CommodityId,
                MarketId = req.MarketId,
                UserId = uid,
                Price = req.Price,
                Unit = req.Unit,
                Note = note,
                ObservedAt = observed
            };
            db.Reports.Add(report);
            await db.SaveChangesAsync();

            return Results.Created($"/api/reports/{report.Id}", new { report.Id });
        });

        g.MapGet("/me/reports", async (HttpContext http, AppDbContext db) =>
        {
            var uid = http.User.GetUserId()!.Value;
            var list = await db.Reports.AsNoTracking()
                .Where(r => r.UserId == uid)
                .OrderByDescending(r => r.ObservedAt)
                .Take(100)
                .Select(r => new { r.Id, r.CommodityId, r.MarketId, r.Price, r.Unit, r.Note, r.ObservedAt })
                .ToListAsync();
            return Results.Ok(list);
        });
    }
}
