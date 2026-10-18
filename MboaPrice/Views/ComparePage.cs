using MboaPrice.Services;
namespace MboaPrice.Views;
public class ComparePage : ContentPage
{
    public ComparePage()
    {
        var picker=new Picker{Title="Choisir une denrée",ItemsSource=DemoDataService.Foods.Select(x=>x.Name).ToList(),SelectedIndex=0};
        var list=new VerticalStackLayout();
        void Refresh(){list.Children.Clear();foreach(var m in new[]{("Mboppi",700),("Marché Central",750),("New Bell",780),("Deïdo",800)})list.Children.Add(Ui.Card(new VerticalStackLayout{Children={new Label{Text=m.Item1,FontAttributes=FontAttributes.Bold,TextColor=Color.FromArgb("#183B30")},new Label{Text=$"{m.Item2} FCFA / kg",FontSize=20,TextColor=Color.FromArgb("#217A59")},new ProgressBar{Progress=m.Item2/850.0,ProgressColor=Color.FromArgb("#217A59")}}}));}
        picker.SelectedIndexChanged+=(_,_)=>Refresh();
        Content=Ui.Scroll(Ui.Heading("Comparer"),Ui.Muted("Le même produit, les mêmes unités"),picker,Ui.Card(new Label{Text="⌖ Douala                         "+DateTime.Today.ToString("dd MMM yyyy"),TextColor=Color.FromArgb("#217A59")}),Ui.Card(new VerticalStackLayout{Children={Ui.Muted("MBOPPI EST LE MOINS CHER"),Ui.Heading("100 FCFA / kg d'écart",23),Ui.Muted("Soit 12,5 % de moins qu'à Deïdo.")},BackgroundColor=Color.FromArgb("#174D3A")}),Ui.Heading("Moyennes · 4 marchés",18),list,Ui.Muted("Prix de démonstration : vérifiez les prix au marché avant achat."));
        Refresh();
    }
}
