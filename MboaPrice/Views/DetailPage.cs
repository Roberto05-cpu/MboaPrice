using MboaPrice.Services;
namespace MboaPrice.Views;
[QueryProperty(nameof(FoodName),"foodName")]
public class DetailPage : ContentPage
{
    string foodName="Riz long grain";
    public string FoodName { get=>foodName; set { foodName=value; Build(); } }
    public DetailPage(){Build();}
    void Build()
    {
        var f=DemoDataService.Foods.FirstOrDefault(x=>x.Name==foodName)??DemoDataService.Foods[0];
        var markets=new VerticalStackLayout{Spacing=0};
        foreach(var item in new[]{("Mboppi",700),("Marché Central",750),("New Bell",780),("Deïdo",800)})
            markets.Children.Add(Ui.Card(new Grid{ColumnDefinitions={new ColumnDefinition(GridLength.Star),new ColumnDefinition(GridLength.Auto)},Children={new Label{Text="♧  "+item.Item1,TextColor=Color.FromArgb("#183B30"),VerticalOptions=LayoutOptions.Center},new Label{Text=$"{item.Item2} FCFA / {f.Unit}",FontAttributes=FontAttributes.Bold,TextColor=Color.FromArgb("#217A59"),HorizontalOptions=LayoutOptions.End}}}));
        Content=Ui.Scroll(new Label{Text="‹  Retour",TextColor=Color.FromArgb("#217A59")},Ui.Heading(f.Name),Ui.Muted(f.Category+" · Unité de référence : "+f.Unit),Ui.Card(new VerticalStackLayout{Children={Ui.Muted("Moyenne à Douala"),Ui.Heading($"{f.AveragePrice:N0} FCFA / {f.Unit}",26),new Label{Text=f.Change+" depuis le 29 sept.",TextColor=Color.FromArgb("#217A59")}}}),new HorizontalStackLayout{Children={Ui.Heading("Prix par marché",18),new Label{Text="Comparer",TextColor=Color.FromArgb("#217A59"),HorizontalOptions=LayoutOptions.EndAndExpand,VerticalOptions=LayoutOptions.Center}}},markets,Ui.Heading("Dernier prix observé",18),Ui.Card(new Label{Text="Mboppi · 06 oct., 08:30       720 FCFA / "+f.Unit,TextColor=Color.FromArgb("#B95F3C")}),Ui.GreenButton("+ Ajouter un relevé pour ce produit",async()=>await Shell.Current.GoToAsync("//Ajouter")));
    }
}
