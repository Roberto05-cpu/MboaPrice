using MboaPrice.Models;
namespace MboaPrice.Services;
public static class DemoDataService
{
    public static List<FoodItem> Foods { get; } = new()
    {
        new() { Name="Riz long grain", Category="Céréales", AveragePrice=758, Unit="kg", Change="−5,3 %", Emoji="🍚", Description="Riz vendu en vrac, prix observés pour 1 kg.", IsFavorite=true },
        new() { Name="Tomate fraîche", Category="Légumes", AveragePrice=1100, Unit="kg", Change="−8,3 %", Emoji="🍅", IsFavorite=true },
        new() { Name="Huile raffinée", Category="Huiles", AveragePrice=1450, Unit="L", Change="+3,6 %", Emoji="🫙" },
        new() { Name="Oignon rouge", Category="Légumes", AveragePrice=900, Unit="kg", Change="−2,2 %", Emoji="🧅" },
        new() { Name="Plantain", Category="Légumes", AveragePrice=600, Unit="kg", Change="−4,8 %", Emoji="🍌" },
        new() { Name="Farine de blé", Category="Céréales", AveragePrice=650, Unit="kg", Change="0,0 %", Emoji="🌾" }
    };
    public static List<PriceReport> Reports { get; } = new();
}
public class PriceReport
{
    public string Food { get; set; } = "";
    public string Market { get; set; } = "";
    public int Price { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public string Unit { get; set; } = "kg";
}
