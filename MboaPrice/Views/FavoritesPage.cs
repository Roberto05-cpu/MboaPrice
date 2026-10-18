using MboaPrice.Services;
namespace MboaPrice.Views;
public class FavoritesPage : ContentPage
{
    public FavoritesPage()
    {
        var foods=new VerticalStackLayout{Spacing=0};
        void Refresh(){foods.Children.Clear();var fav=DemoDataService.Foods.Where(f=>f.IsFavorite).ToList();if(fav.Count==0)foods.Children.Add(Ui.Muted("Aucun favori pour le moment."));foreach(var f in fav){var card=HomePage.FoodCard(f);var tap=new TapGestureRecognizer();tap.Tapped+=async(_,_)=>await Shell.Current.GoToAsync(nameof(DetailPage),new Dictionary<string,object>{{"foodName",f.Name}});card.GestureRecognizers.Add(tap);foods.Children.Add(card);}}
        Content=new ScrollView{Content=Ui.PageStack(Ui.Heading("Vos favoris"),Ui.Muted("Vos repères, toujours à portée de main"),Ui.Card(new Label{Text="Tout · 4     Denrées · 2     Marchés · 2",TextColor=Color.FromArgb("#217A59")}),new HorizontalStackLayout{Children={Ui.Heading("Mes denrées",18),new Label{Text="Gérer",TextColor=Color.FromArgb("#217A59"),HorizontalOptions=LayoutOptions.EndAndExpand}}},foods,Ui.Card(new Label{Text="ⓘ  Touchez le cœur d'une denrée ou d'un marché pour la retrouver ici.",TextColor=Color.FromArgb("#217A59"),FontSize=12}))};
        Refresh();
    }
}
