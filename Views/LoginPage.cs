namespace MboaPrice.Views;
public class LoginPage : ContentPage
{
    public LoginPage()
    {
        var email=Ui.Field("vous@exemple.com");var password=Ui.Field("Mot de passe");password.IsPassword=true;
        Content=Ui.Scroll(new Label{Text="🌱  MboaPrice",FontSize=22,FontAttributes=FontAttributes.Bold,TextColor=Color.FromArgb("#217A59")},new Grid{HeightRequest=110,Children= {new Image { Source = "Resources/Images/market_douala.jpg", Aspect = Aspect.AspectFill }, new Label{Text="Au plus près des marchés de Douala",TextColor=Colors.White,FontAttributes=FontAttributes.Bold,VerticalTextAlignment=TextAlignment.Center,HorizontalTextAlignment=TextAlignment.Center,Padding=18}}},Ui.Heading("Les bons prix, près de vous.",27),Ui.Muted("Comparez les denrées. Préparez vos achats. Partagez les prix de votre marché."),new Label{Text="Adresse e-mail"},email,new Label{Text="Mot de passe"},password,new Label{Text="Mot de passe oublié ?",TextColor=Color.FromArgb("#217A59"),HorizontalTextAlignment=TextAlignment.End},Ui.GreenButton("Se connecter",async()=>await Shell.Current.GoToAsync("//Accueil")),new Button{Text="Explorer sans compte",BackgroundColor=Colors.White,TextColor=Color.FromArgb("#217A59"),Command=new Command(async()=>await Shell.Current.GoToAsync("//Accueil"))},new HorizontalStackLayout{Children={Ui.Muted("Nouveau ici ?"),new Label{Text="Créer un compte",TextColor=Color.FromArgb("#217A59"),GestureRecognizers={new TapGestureRecognizer{Command=new Command(async()=>await Shell.Current.GoToAsync($"///{nameof(RegisterPage)}"))}}}}});
    }
}