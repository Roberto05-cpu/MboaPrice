//using MboaPrice.Models;
//using MboaPrice.Services;
//namespace MboaPrice.Views;
//public class HomePage : ContentPage
//{
//    public HomePage()
//    {
//        var top = new Grid { ColumnDefinitions={new ColumnDefinition(GridLength.Star),new ColumnDefinition(GridLength.Auto)} };
//        top.Add(new VerticalStackLayout { Children={Ui.Heading("Bonjour, Ariane",24),Ui.Muted("Votre repère prix à Douala")} },0,0);
//        var bell = new Button { Text="♧", BackgroundColor=Colors.White, TextColor=Color.FromArgb("#217A59"), Padding=8, WidthRequest=44 };
//        top.Add(bell,1,0);
//        var location=Ui.Card(new HorizontalStackLayout { Children={new Label{Text="⌖  Douala",TextColor=Color.FromArgb("#217A59"),FontAttributes=FontAttributes.Bold},new Label{Text=DateTime.Today.ToString("dd MMM yyyy"),HorizontalOptions=LayoutOptions.EndAndExpand,TextColor=Color.FromArgb("#718078")}}});
//        var heroContent = new VerticalStackLayout { Spacing=8, Children={new Label{Text="Riz long grain · Prix moyen",TextColor=Colors.White},new Label{Text="758 FCFA / kg",FontSize=28,FontAttributes=FontAttributes.Bold,TextColor=Colors.White},new Label{Text="↘  −5,3 % depuis le 29 sept.   ·   12 relevés · 4 marchés",TextColor=Color.FromArgb("#D7E9DD"),FontSize=12},new Button{Text="Voir l'évolution des prix",BackgroundColor=Colors.White,TextColor=Color.FromArgb("#174D3A"),Command=new Command(async()=>await Shell.Current.GoToAsync(nameof(TrendsPage)))}}};
//        var hero = new Border { Content=heroContent,BackgroundColor=Color.FromArgb("#174D3A"),Stroke=Colors.Transparent,StrokeShape=new Microsoft.Maui.Controls.Shapes.RoundRectangle{CornerRadius=18},Padding=18 };
//        var list=new VerticalStackLayout{Spacing=0};
//        foreach(var food in DemoDataService.Foods.Take(3)) list.Children.Add(FoodCard(food));
//        Content=new ScrollView{Content=Ui.PageStack(top,location,hero,new HorizontalStackLayout{Children={Ui.Heading("Les essentiels du jour",18),new Label{Text="Tout voir",TextColor=Color.FromArgb("#217A59"),HorizontalOptions=LayoutOptions.EndAndExpand,VerticalOptions=LayoutOptions.Center}}},list,Ui.Card(new HorizontalStackLayout{Children={new Label{Text="✎",FontSize=22,TextColor=Color.FromArgb("#217A59")},new VerticalStackLayout{Children={new Label{Text="Un prix vu au marché ?",FontAttributes=FontAttributes.Bold,TextColor=Color.FromArgb("#183B30")},Ui.Muted("Ajoutez un relevé pour la communauté.")}},Spacing=12}}))};
//    }
//    public static Border FoodCard(FoodItem f)
//    {
//        var row=new Grid{ColumnDefinitions={new ColumnDefinition(new GridLength(48)),new ColumnDefinition(GridLength.Star),new ColumnDefinition(GridLength.Auto)},ColumnSpacing=10};
//        row.Add(new Border{Content=new Label{Text=f.Emoji,FontSize=25,HorizontalTextAlignment=TextAlignment.Center,VerticalTextAlignment=TextAlignment.Center},BackgroundColor=Color.FromArgb("#F4F1E8"),Stroke=Colors.Transparent,StrokeShape=new Microsoft.Maui.Controls.Shapes.RoundRectangle{CornerRadius=10},WidthRequest=48,HeightRequest=48},0,0);
//        row.Add(new VerticalStackLayout{Spacing=3,VerticalOptions=LayoutOptions.Center,Children={new Label{Text=f.Name,FontAttributes=FontAttributes.Bold,TextColor=Color.FromArgb("#183B30")},new Label{Text=$"{f.AveragePrice:N0} FCFA / {f.Unit}",TextColor=Color.FromArgb("#183B30"),FontSize=12},Ui.Muted("12 relevés · 4 marchés",10)}},1,0);
//        row.Add(new Label{Text=f.Change,TextColor=Color.FromArgb("#217A59"),FontSize=12,VerticalOptions=LayoutOptions.Center},2,0);
//        var b=Ui.Card(row); var tap=new TapGestureRecognizer(); tap.Tapped+=async(_,_)=>await Shell.Current.GoToAsync(nameof(DetailPage),new Dictionary<string,object>{{"foodName",f.Name}}); b.GestureRecognizers.Add(tap); return b;
//}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MboaPrice.Models;
using MboaPrice.Services;

namespace MboaPrice.Views;

public class HomePage : ContentPage
{
    public HomePage()
    {
        // En-tête
        var top = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        var welcome = new VerticalStackLayout
        {
            Spacing = 4
        };

        welcome.Children.Add(Ui.Heading("Bonjour, Ariane", 24));
        welcome.Children.Add(Ui.Muted("Votre repère prix à Douala"));

        top.Add(welcome, 0, 0);

        var bell = new Button
        {
            Text = "♧",
            BackgroundColor = Colors.White,
            TextColor = Color.FromArgb("#217A59"),
            Padding = 8,
            WidthRequest = 44
        };

        top.Add(bell, 1, 0);

        // Localisation et date
        var locationRow = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        locationRow.Add(
            new Label
            {
                Text = "⌖  Douala",
                TextColor = Color.FromArgb("#217A59"),
                FontAttributes = FontAttributes.Bold
            },
            0, 0
        );

        locationRow.Add(
            new Label
            {
                Text = DateTime.Today.ToString("dd MMM yyyy"),
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Color.FromArgb("#718078")
            },
            1, 0
        );

        var location = Ui.Card(locationRow);

        // Carte principale des prix
        var heroContent = new VerticalStackLayout
        {
            Spacing = 8
        };

        heroContent.Children.Add(new Label
        {
            Text = "Riz long grain · Prix moyen",
            TextColor = Colors.White
        });

        heroContent.Children.Add(new Label
        {
            Text = "758 FCFA / kg",
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White
        });

        heroContent.Children.Add(new Label
        {
            Text = "↘  −5,3 % depuis le 29 sept. · 12 relevés · 4 marchés",
            TextColor = Color.FromArgb("#D7E9DD"),
            FontSize = 12
        });

        heroContent.Children.Add(new Button
        {
            Text = "Voir l'évolution des prix",
            BackgroundColor = Colors.White,
            TextColor = Color.FromArgb("#174D3A"),
            Command = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(TrendsPage));
            })
        });

        var hero = new Border
        {
            Content = heroContent,
            BackgroundColor = Color.FromArgb("#174D3A"),
            Stroke = Colors.Transparent,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = 18
            },
            Padding = 18
        };

        // Liste des produits
        var list = new VerticalStackLayout
        {
            Spacing = 0
        };

        foreach (var food in DemoDataService.Foods.Take(3))
        {
            list.Children.Add(FoodCard(food));
        }

        // Titre de la section
        var sectionHeader = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        sectionHeader.Add(Ui.Heading("Les essentiels du jour", 18), 0, 0);

        sectionHeader.Add(
            new Label
            {
                Text = "Tout voir",
                TextColor = Color.FromArgb("#217A59"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            },
            1, 0
        );

        // Invitation à contribuer
        var contributionContent = new HorizontalStackLayout
        {
            Spacing = 12
        };

        contributionContent.Children.Add(new Label
        {
            Text = "✎",
            FontSize = 22,
            TextColor = Color.FromArgb("#217A59"),
            VerticalOptions = LayoutOptions.Center
        });

        var contributionText = new VerticalStackLayout
        {
            Spacing = 4
        };

        contributionText.Children.Add(new Label
        {
            Text = "Un prix vu au marché ?",
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#183B30")
        });

        contributionText.Children.Add(
            Ui.Muted("Ajoutez un relevé pour la communauté.")
        );

        contributionContent.Children.Add(contributionText);

        var contributionCard = Ui.Card(contributionContent);

        // Contenu de la page
        Content = new ScrollView
        {
            Content = Ui.PageStack(
                top,
                location,
                hero,
                sectionHeader,
                list,
                contributionCard
            )
        };
    }

    public static Border FoodCard(FoodItem f)
    {
        var row = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(48) },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            ColumnSpacing = 10
        };

        // Icône
        row.Add(
            new Border
            {
                Content = new Label
                {
                    Text = f.Emoji,
                    FontSize = 25,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                },
                BackgroundColor = Color.FromArgb("#F4F1E8"),
                Stroke = Colors.Transparent,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = 10
                },
                WidthRequest = 48,
                HeightRequest = 48
            },
            0, 0
        );

        // Informations du produit
        var details = new VerticalStackLayout
        {
            Spacing = 3,
            VerticalOptions = LayoutOptions.Center
        };

        details.Children.Add(new Label
        {
            Text = f.Name,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#183B30")
        });

        details.Children.Add(new Label
        {
            Text = $"{f.AveragePrice:N0} FCFA / {f.Unit}",
            TextColor = Color.FromArgb("#183B30"),
            FontSize = 12
        });

        details.Children.Add(Ui.Muted("12 relevés · 4 marchés", 10));

        row.Add(details, 1, 0);

        // Évolution du prix
        row.Add(
            new Label
            {
                Text = f.Change,
                TextColor = Color.FromArgb("#217A59"),
                FontSize = 12,
                VerticalOptions = LayoutOptions.Center
            },
            2, 0
        );

        var card = Ui.Card(row);

        var tap = new TapGestureRecognizer();

        tap.Tapped += async (_, _) =>
        {
            await Shell.Current.GoToAsync(
                $"///{nameof(DetailPage)}",  
                new Dictionary<string, object>
                {
                    { "foodName", f.Name }
                }
            );
        };

        card.GestureRecognizers.Add(tap);

        return card;
    }
}
