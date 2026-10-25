namespace MboaPrice.Api.Data;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>Une denrée (riz, tomate…). Id = identifiant lisible (« riz »).</summary>
public class Commodity
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Unit { get; set; } = "kg";
    public string UnitWord { get; set; } = "kilogramme";
    public string Emoji { get; set; } = "";
    public string Demonstrative { get; set; } = "";
    public string ThumbColor { get; set; } = "#EFE9DC";
    public int SortOrder { get; set; }
}

public class Market
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string City { get; set; } = "Douala";
    public int SortOrder { get; set; }
}

/// <summary>Un prix vu par un utilisateur. Les moyennes sont calculées à partir de ces relevés.</summary>
public class PriceReport
{
    public int Id { get; set; }
    public string CommodityId { get; set; } = "";
    public string MarketId { get; set; } = "";
    public int? UserId { get; set; }
    public int Price { get; set; }
    public string Unit { get; set; } = "kg";
    public string Note { get; set; } = "";
    public DateTime ObservedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class FavoriteCommodity
{
    public int UserId { get; set; }
    public string CommodityId { get; set; } = "";
}

public class FavoriteMarket
{
    public int UserId { get; set; }
    public string MarketId { get; set; } = "";
}
