using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Game1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UltimateGamePage : ContentPage
    {
        private int[,] buttonsStates = new int[9, 9];
        List<List<Button>> buttons = new List<List<Button>>();


        public UltimateGamePage()
        {
            InitializeComponent();
            CreateField();
            BackgroundColor = Color.FromHex("#1a1a1a");
        }

        private void CreateField()
        {
            Field.Children.Clear();
            StackLayout FieldVt = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 15,
                VerticalOptions = LayoutOptions.Center
            };
            List<Button> btns = new List<Button>();
            int c1 = 1;
            for (int i = 1; i <= 3; i++)
            {
                StackLayout FieldHr = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 15,
                    HorizontalOptions = LayoutOptions.Center
                };
                for (int d = 1; d <= 3; d++)
                {
                    StackLayout stVert = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical
                    };

                    int c2 = 1;
                    for (int j = 1; j <= 3; j++)
                    {
                        StackLayout stHoriz = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal
                        };
                        for (int k = 1; k <= 3; k++)
                        {
                            Button btn = new Button
                            {
                                Text = "",
                                TextColor = Color.Black,
                                BackgroundColor = Color.WhiteSmoke,
                                StyleId = $"btn{c1}{c2}",
                                WidthRequest = 34,
                                HeightRequest = 34
                            };
                            btn.Clicked += Button_Clicked;
                            stHoriz.Children.Add(btn);
                            c2++;
                        }
                        stVert.Children.Add(stHoriz);
                    }
                    FieldHr.Children.Add(stVert);
                    c1++;
                }
                FieldVt.Children.Add(FieldHr);
            }
            Field.Children.Add(FieldVt);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}