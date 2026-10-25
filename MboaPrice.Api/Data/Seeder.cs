using Microsoft.AspNetCore.Identity;

namespace MboaPrice.Api.Data;

/// <summary>Données de départ : denrées, marchés, ~40 jours de relevés et un compte de démonstration.</summary>
public static class Seeder
{
    private static readonly (string Id, string Name)[] MarketDefs =
    {
        ("mboppi", "Mboppi"), ("marche-central", "Marché Central"), ("new-bell", "New Bell"), ("deido", "Deïdo")
    };
    private static readonly double[] Factors = { 0.9235, 0.9894, 1.0290, 1.0554 };

    // Id, nom, catégorie, unité, mot, emoji, démonstratif, couleur, prix moyen cible, nb marchés, variation sur 7 jours (%)
    private record Def(string Id, string Name, string Category, string Unit, string UnitWord, string Emoji,
        string Demonstrative, string Thumb, int Avg, int Markets, double Change);

    private static readonly Def[] Defs =
    {
        new("riz", "Riz long grain", "Céréales", "kg", "kilogramme", "🍚", "ce riz", "#EFE9DC", 758, 4, -5.3),
        new("tomate", "Tomate fraîche", "Légumes", "kg", "kilogramme", "🍅", "cette tomate", "#F6DCD5", 1100, 4, -8.3),
        new("huile", "Huile raffinée", "Huiles", "L", "litre", "🫒", "cette huile", "#F5EBC8", 1450, 4, 3.6),
        new("oignon", "Oignon rouge", "Légumes", "kg", "kilogramme", "🧅", "cet oignon", "#EBD4DD", 900, 3, -2.2),
        new("plantain", "Plantain", "Légumes", "kg", "kilogramme", "🍌", "ce plantain", "#F3E7B8", 600, 3, -4.8),
        new("farine", "Farine de blé", "Céréales", "kg", "kilogramme", "🌾", "cette farine", "#F1ECE2", 650, 3, 0.0),
    };

    public static void Run(AppDbContext db, IPasswordHasher<User> hasher)
    {
        if (db.Commodities.Any()) return;

        var today = Clock.Today;

        db.Markets.AddRange(MarketDefs.Select((m, i) => new Market { Id = m.Id, Name = m.Name, SortOrder = i }));
        db.Commodities.AddRange(Defs.Select((d, i) => new Commodity
        {
            Id = d.Id, Name = d.Name, Category = d.Category, Unit = d.Unit, UnitWord = d.UnitWord, Emoji = d.Emoji,
            Demonstrative = d.Demonstrative, ThumbColor = d.Thumb, SortOrder = i
        }));

        var demo = new User { FullName = "Ariane Ndom", Email = "ariane.ndom@exemple.com" };
        demo.PasswordHash = hasher.HashPassword(demo, "Demo1234");
        db.Users.Add(demo);
        db.SaveChanges();

        // Un relevé par marché et par jour sur 40 jours. Aucun « bruit » sur les 10 derniers jours
        // pour que les moyennes affichées correspondent aux maquettes.
        var rnd = new Random(42);
        foreach (var d in Defs)
        {
            var startRatio = 1 / (1 + d.Change / 100.0);
            for (var daysAgo = 40; daysAgo >= 0; daysAgo--)
            {
                var ramp = Math.Clamp((daysAgo - 2) / 5.0, 0, 1);
                var drift = daysAgo > 9 ? 0.003 * (daysAgo - 9) * (d.Change <= 0 ? 1 : -1) : 0;
                var noise = daysAgo > 9 ? (rnd.NextDouble() - 0.5) * 0.012 : 0;

                for (var m = 0; m < d.Markets; m++)
                {
                    var basePrice = d.Id == "riz" ? new[] { 700, 750, 780, 800 }[m] : d.Avg * Factors[m];
                    var raw = basePrice * (1 + (startRatio - 1) * ramp + drift + noise);
                    db.Reports.Add(new PriceReport
                    {
                        CommodityId = d.Id,
                        MarketId = MarketDefs[m].Id,
                        Price = (int)(Math.Round(raw / 5.0) * 5),
                        Unit = d.Unit,
                        Note = "Relevé individuel",
                        ObservedAt = today.AddDays(-daysAgo).AddHours(8.5)
                    });
                }
            }
        }

        // Relevés du compte de démonstration (anciens : ils ne changent pas les moyennes du jour).
        foreach (var (commodity, market, price, daysAgo, note) in new[]
        {
            ("riz", "mboppi", 720, 3, "Riz vendu en vrac, prix pour 1 kg."),
            ("tomate", "marche-central", 1150, 12, "Tomates du jour"),
            ("huile", "new-bell", 1500, 14, "Bidon d'1 litre"),
            ("oignon", "deido", 950, 15, "Filet d'oignons"),
            ("plantain", "mboppi", 650, 18, "Régime de plantain"),
            ("farine", "marche-central", 700, 20, "Sachet d'1 kg"),
            ("riz", "new-bell", 790, 22, "Riz importé"),
            ("tomate", "mboppi", 1050, 25, "Prix du matin"),
        })
        {
            db.Reports.Add(new PriceReport
            {
                CommodityId = commodity, MarketId = market, UserId = demo.Id, Price = price,
                Unit = Defs.First(x => x.Id == commodity).Unit, Note = note,
                ObservedAt = today.AddDays(-daysAgo).AddHours(9)
            });
        }

        db.FavoriteCommodities.AddRange(
            new FavoriteCommodity { UserId = demo.Id, CommodityId = "riz" },
            new FavoriteCommodity { UserId = demo.Id, CommodityId = "tomate" });
        db.FavoriteMarkets.AddRange(
            new FavoriteMarket { UserId = demo.Id, MarketId = "mboppi" },
            new FavoriteMarket { UserId = demo.Id, MarketId = "marche-central" });

        db.SaveChanges();
    }
}
