using MboaPrice.Services;
namespace MboaPrice.Views;
public class CatalogPage : ContentPage
{
    VerticalStackLayout results = new(){Spacing=0};
    string category="Tout";
    public CatalogPage()
    {
        var search=Ui.Field("⌕  Rechercher une denrée");
        search.TextChanged+=(_,_)=>Refresh(search.Text);
        var filters=new HorizontalStackLayout{Spacing=8};
        foreach(var c in new[]{"Tout","Céréales","Légumes","Huiles"}) { var b=new Button{Text=c,FontSize=12,Padding=10,BackgroundColor=c=="Tout"?Color.FromArgb("#217A59"):Colors.White,TextColor=c=="Tout"?Colors.White:Color.FromArgb("#183B30")}; b.Clicked+=(_,_)=>{category=c; foreach(var x in filters.Children.OfType<Button>()){x.BackgroundColor=x.Text==c?Color.FromArgb("#217A59"):Colors.White;x.TextColor=x.Text==c?Colors.White:Color.FromArgb("#183B30");} Refresh(search.Text);}; filters.Children.Add(b); }
        Content=new ScrollView{Content=Ui.PageStack(Ui.Heading("Les denrées"),Ui.Muted("Des prix clairs pour vos achats"),search,filters,Ui.Card(new Label{Text="⌖  Douala                      "+DateTime.Today.ToString("dd MMM yyyy"),TextColor=Color.FromArgb("#217A59")}),Ui.Muted("6 denrées disponibles · Prix moyens · FCFA"),results)};
        Refresh("");
    }
    void Refresh(string? query){results.Children.Clear();foreach(var f in DemoDataService.Foods.Where(f=>(category=="Tout"||f.Category==category)&&(string.IsNullOrWhiteSpace(query)||f.Name.Contains(query,StringComparison.OrdinalIgnoreCase))))results.Children.Add(HomePage.FoodCard(f));}
}
