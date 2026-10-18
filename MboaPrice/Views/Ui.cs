using Microsoft.Maui.Controls;
namespace MboaPrice.Views;
public static class Ui
{
    public static Label Heading(string text, double size=25) => new() { Text=text, FontSize=size, FontAttributes=FontAttributes.Bold, TextColor=Color.FromArgb("#183B30") };
    public static Label Muted(string text, double size=12) => new() { Text=text, FontSize=size, TextColor=Color.FromArgb("#718078") };
    public static Border Card(View content) => new() { Content=content, BackgroundColor=Colors.White, Stroke=Color.FromArgb("#E6EBE3"), StrokeThickness=1, StrokeShape=new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius=16 }, Padding=14, Margin=new Thickness(0,5) };
    public static Entry Field(string placeholder) => new() { Placeholder=placeholder, BackgroundColor=Colors.White, HeightRequest=48 };
    public static VerticalStackLayout PageStack(params View[] children) { var s=new VerticalStackLayout { Padding=new Thickness(18,16), Spacing=12 }; foreach(var c in children) s.Children.Add(c); return s; }
    public static ScrollView Scroll(params View[] children) => new() { Content=PageStack(children) };
    public static Button GreenButton(string text, Action action) { var b=new Button { Text=text }; b.Clicked += (_,_)=>action(); return b; }
}
