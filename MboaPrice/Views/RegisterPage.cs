namespace MboaPrice.Views;
public class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        var name=Ui.Field("Ariane Ndom");var email=Ui.Field("ariane.ndom@example.com");var pass=Ui.Field("8 caractères minimum");pass.IsPassword=true;var confirm=Ui.Field("Confirmer le mot de passe");confirm.IsPassword=true;
        var consent=new CheckBox{Color=Color.FromArgb("#217A59")};
        Content=Ui.Scroll(new Label{Text="← Retour                         🌱 MboaPrice",TextColor=Color.FromArgb("#217A59")},Ui.Heading("Ensemble, achetons mieux.",27),Ui.Muted("Créez votre compte pour enregistrer vos favoris et contribuer aux prix de Douala."),new Label{Text="Nom complet"},name,new Label{Text="Adresse e-mail"},email,new Label{Text="Mot de passe"},pass,new Label{Text="Confirmer le mot de passe"},confirm,new HorizontalStackLayout{Children={consent,new Label{Text="J'accepte les conditions d'utilisation et la politique de confidentialité.",FontSize=12,TextColor=Color.FromArgb("#183B30"),VerticalOptions=LayoutOptions.Center}}},Ui.GreenButton("Créer mon compte",async()=>{if(string.IsNullOrWhiteSpace(name.Text)||string.IsNullOrWhiteSpace(email.Text)||pass.Text?.Length<8||pass.Text!=confirm.Text||!consent.IsChecked){await DisplayAlert("Vérification","Complétez les champs, utilisez un mot de passe d'au moins 8 caractères et acceptez les conditions.","OK");return;}await DisplayAlert("Compte de démonstration","Le backend n'est pas encore connecté. Aucun compte n'a été créé sur un serveur.","OK");await Shell.Current.GoToAsync("//Accueil");}),Ui.Muted("Vos contributions aident la communauté. Votre adresse e-mail reste privée."),new Label{Text="Déjà un compte ? Se connecter",TextColor=Color.FromArgb("#217A59"),HorizontalTextAlignment=TextAlignment.Center,GestureRecognizers={new TapGestureRecognizer{Command=new Command(async()=>await Shell.Current.GoToAsync($"///{nameof(LoginPage)}"))}}});
    }
}
