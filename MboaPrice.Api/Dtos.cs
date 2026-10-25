namespace MboaPrice.Api;

// Contrat JSON partagé avec l'application MAUI (MboaPrice/Services/Dtos.cs). Garder les mêmes noms.

public record OverviewDto(DateTime ReferenceDate, string City, List<CommodityDto> Commodities, List<MarketDto> Markets, int MyReportsCount);

public record CommodityDto(
    string Id, string Name, string Category, string Unit, string UnitWord, string Emoji, string Demonstrative,
    string ThumbColor, int AveragePrice, int SampleCount, int MarketCount, double ChangePercent, bool IsFavorite,
    List<MarketPriceDto> MarketPrices, List<DailyPointDto> History, LastReportDto? LastReport);

public record MarketDto(string Id, string Name, string City, bool IsFavorite);
public record MarketPriceDto(string Market, int Price, int ReportCount);
public record DailyPointDto(DateTime Date, int Price);
public record LastReportDto(string Market, DateTime At, int Price, string Unit, string Note);

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string FullName, string Email, string Password);
public record UserDto(int Id, string FullName, string Email);
public record AuthResponse(string Token, UserDto User);
public record CreateReportRequest(string CommodityId, string MarketId, int Price, string Unit, string? Note, DateTime ObservedAt);
public record ApiError(string Error);
