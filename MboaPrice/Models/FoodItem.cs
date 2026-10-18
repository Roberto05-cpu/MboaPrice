namespace MboaPrice.Models;
public class FoodItem
{
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public int AveragePrice { get; set; }
    public string Unit { get; set; } = "kg";
    public string Change { get; set; } = "0,0 %";
    public string Emoji { get; set; } = "🥬";
    public string Description { get; set; } = "";
    public bool IsFavorite { get; set; }
}
