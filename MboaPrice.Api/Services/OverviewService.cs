using MboaPrice.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MboaPrice.Api.Services;

/// <summary>
/// Calcule tout ce que l'application affiche à partir des relevés :
/// moyenne (relevés des 3 derniers jours), variation vs 7 jours plus tôt, prix par marché, historique.
/// </summary>
public class OverviewService(AppDbContext db)
{
    public async Task<OverviewDto> BuildAsync(int? userId, CancellationToken ct = default)
    {
        var today = Clock.Today;
        var from = today.AddDays(-40);

        var commodities = await db.Commodities.AsNoTracking().OrderBy(c => c.SortOrder).ToListAsync(ct);
        var markets = await db.Markets.AsNoTracking().OrderBy(m => m.SortOrder).ToListAsync(ct);
        var reports = await db.Reports.AsNoTracking().Where(r => r.ObservedAt >= from).ToListAsync(ct);

        var favCommodities = new HashSet<string>();
        var favMarkets = new HashSet<string>();
        var myReports = 0;
        if (userId is int uid)
        {
            favCommodities = (await db.FavoriteCommodities.Where(f => f.UserId == uid).Select(f => f.CommodityId).ToListAsync(ct)).ToHashSet();
            favMarkets = (await db.FavoriteMarkets.Where(f => f.UserId == uid).Select(f => f.MarketId).ToListAsync(ct)).ToHashSet();
            myReports = await db.Reports.CountAsync(r => r.UserId == uid, ct);
        }

        var marketsById = markets.ToDictionary(m => m.Id);
        var byCommodity = reports.GroupBy(r => r.CommodityId).ToDictionary(g => g.Key, g => g.ToList());

        var items = commodities.Select(c =>
            Build(c, byCommodity.GetValueOrDefault(c.Id) ?? new List<PriceReport>(), marketsById, today, favCommodities.Contains(c.Id))).ToList();

        return new OverviewDto(today, "Douala", items,
            markets.Select(m => new MarketDto(m.Id, m.Name, m.City, favMarkets.Contains(m.Id))).ToList(),
            myReports);
    }

    private static CommodityDto Build(Commodity c, List<PriceReport> reports, Dictionary<string, Market> markets, DateTime today, bool isFavorite)
    {
        // Fenêtre courante : aujourd'hui et les 2 jours précédents ; sinon les 14 derniers jours.
        var current = reports.Where(r => r.ObservedAt.Date >= today.AddDays(-2) && r.ObservedAt.Date <= today).ToList();
        if (current.Count == 0) current = reports.Where(r => r.ObservedAt >= today.AddDays(-14)).ToList();

        // Fenêtre de comparaison : il y a 7 à 9 jours.
        var previous = reports.Where(r => r.ObservedAt.Date >= today.AddDays(-9) && r.ObservedAt.Date <= today.AddDays(-7)).ToList();

        var average = current.Count > 0 ? current.Average(r => r.Price) : 0;
        var previousAverage = previous.Count > 0 ? previous.Average(r => r.Price) : average;
        var change = previousAverage > 0
            ? Math.Round((average - previousAverage) * 100 / previousAverage, 1, MidpointRounding.AwayFromZero)
            : 0;

        var marketPrices = current
            .GroupBy(r => r.MarketId)
            .Where(g => markets.ContainsKey(g.Key))
            .OrderBy(g => markets[g.Key].SortOrder)
            .Select(g => new MarketPriceDto(markets[g.Key].Name, (int)Math.Round(g.Average(r => r.Price)), g.Count()))
            .ToList();

        // Historique : moyenne de chaque jour, la dernière valeur connue est reportée les jours sans relevé.
        var byDay = reports.GroupBy(r => r.ObservedAt.Date).ToDictionary(g => g.Key, g => g.Average(r => r.Price));
        var last = byDay.Count > 0 ? byDay.OrderBy(k => k.Key).First().Value : average;
        var history = new List<DailyPointDto>();
        for (var offset = -30; offset <= 0; offset++)
        {
            var day = today.AddDays(offset);
            if (byDay.TryGetValue(day, out var value)) last = value;
            history.Add(new DailyPointDto(day, (int)Math.Round(last)));
        }
        if (history.Count > 0) history[^1] = history[^1] with { Price = (int)Math.Round(average) };

        // Dernier relevé : celui d'un utilisateur s'il existe, sinon le plus récent.
        var latest = reports.Where(r => r.UserId != null).OrderByDescending(r => r.ObservedAt).ThenByDescending(r => r.Id).FirstOrDefault()
                     ?? reports.OrderByDescending(r => r.ObservedAt).ThenByDescending(r => r.Id).FirstOrDefault();
        var lastReport = latest is not null && markets.TryGetValue(latest.MarketId, out var lm)
            ? new LastReportDto(lm.Name, latest.ObservedAt, latest.Price, latest.Unit,
                string.IsNullOrWhiteSpace(latest.Note) ? "Relevé individuel" : latest.Note)
            : null;

        return new CommodityDto(c.Id, c.Name, c.Category, c.Unit, c.UnitWord, c.Emoji, c.Demonstrative, c.ThumbColor,
            (int)Math.Round(average), current.Count, marketPrices.Count, change, isFavorite, marketPrices, history, lastReport);
    }
}
