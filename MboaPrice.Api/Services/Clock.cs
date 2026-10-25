namespace MboaPrice.Api;

public static class Clock
{
    /// <summary>« Aujourd'hui » à Douala (UTC+1), sans l'heure.</summary>
    public static DateTime Today => DateTime.UtcNow.AddHours(1).Date;
}
