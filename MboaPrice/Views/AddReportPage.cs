using MboaPrice.Services;
namespace MboaPrice.Views;
public class AddReportPage : ContentPage
{
    Picker food=new(){Title="Choisir une denrée"};
    Picker market=new(){Title="Choisir un marché"};
    Entry price=Ui.Field("Prix observé en FCFA");
    Picker unit=new(){Title="Unité"};
    DatePicker date=new(){Date=DateTime.Today,MaximumDate=DateTime.Today};
    Editor note=new(){Placeholder="Précision (facultatif)",AutoSize=EditorAutoSizeOption.TextChanges,BackgroundColor=Colors.White,HeightRequest=75};
    public AddReportPage()
    {
        food.ItemsSource=DemoDataService.Foods.Select(x=>x.Name).ToList(); food.SelectedIndex=0;
        market.ItemsSource=new[]{"Mboppi · Douala","Marché Central","New Bell","Deïdo"}; market.SelectedIndex=0;
        unit.ItemsSource=new[]{"kg","L","unité","sac"};unit.SelectedIndex=0;
        var hint=Ui.Card(new Label{Text="ⓘ  Saisissez le prix réellement vu chez un vendeur, pas une moyenne. Ce formulaire représente un exemple non publié.",TextColor=Color.FromArgb("#217A59"),FontSize=12});
        var publish=Ui.GreenButton("✓  Publier le relevé",async()=>{
            if(food.SelectedItem==null||market.SelectedItem==null||!int.TryParse(price.Text,out var p)||p<=0){await DisplayAlert("Informations manquantes","Choisissez une denrée et un marché, puis saisissez un prix positif.","OK");return;}
            DemoDataService.Reports.Add(new PriceReport{Food=food.SelectedItem.ToString()!,Market=market.SelectedItem.ToString()!,Price=p,Date=date.Date,Unit=unit.SelectedItem?.ToString()??"kg"});
            await DisplayAlert("Relevé enregistré","Le relevé est conservé localement pour cette démonstration.","OK");price.Text="";note.Text="";
        });
        Content=new ScrollView{Content=Ui.PageStack(Ui.Heading("Ajouter un relevé"),Ui.Muted("Un prix observé, une information utile"),hint,new Label{Text="Denrée *"},food,new Label{Text="Marché *"},market,new Label{Text="Date d'observation *"},date,new Label{Text="Prix observé *"},price,new Label{Text="Unité *"},unit,new Label{Text="Précision (facultatif)"},note,publish,Ui.Muted("* Champs obligatoires. Vérifiez la quantité et l'unité avant de partager.") )};
    }
}
