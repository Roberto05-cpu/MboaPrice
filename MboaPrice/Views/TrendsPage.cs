using MboaPrice.Services;
namespace MboaPrice.Views;
public class TrendsPage : ContentPage
{
    public TrendsPage()
    {
        var bars=new VerticalStackLayout{Spacing=14};
        foreach(var (day,val) in new[]{("29 sept.",820),("01 oct.",800),("03 oct.",780),("06 oct.",758)})bars.Children.Add(new VerticalStackLayout{Children={new Label{Text=$"{day}     {val} FCFA / kg",TextColor=Color.FromArgb("#183B30")},new ProgressBar{Progress=val/900.0,ProgressColor=Color.FromArgb("#217A59")}}});
        Content=Ui.Scroll(Ui.Heading("Tendances · Prix moyens"),Ui.Muted("Suivez l'évolution des prix dans le temps"),Ui.Card(new VerticalStackLayout{Children={Ui.Muted("RIZ LONG GRAIN · DOUALA"),Ui.Heading("758 FCFA / kg",28),new Label{Text="−5,3 % sur la période",TextColor=Color.FromArgb("#217A59")}}}),Ui.Heading("Évolution récente",18),bars,Ui.Card(new Label{Text="Les tendances sont calculées à partir de relevés de démonstration enregistrés localement.",TextColor=Color.FromArgb("#217A59"),FontSize=12}));
    }
}
