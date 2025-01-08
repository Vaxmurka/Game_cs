using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Button = Xamarin.Forms.Button;

namespace Game1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UltimateGamePage : ContentPage
    {
        private int[,] buttonStates = new int[9,9];
        private int[] totalStates = new int[9];

        List<List<Button>> buttons = new List<List<Button>>();
        List<Label> labels = new List<Label>();


        public UltimateGamePage()
        {
            InitializeComponent();
            InitializeButtonStates();
            BackgroundColor = Color.FromHex("#1a1a1a");
        }

        private void InitializeButtonStates()
        {
            CreateField();

            // Задаем пустое состояние для всех кнопок
            for (int i = 0; i < buttonStates.GetLength(0); i++)
            {
                for (int j = 0; j < buttonStates.GetLength(1); j++)
                {
                    buttonStates[i, j] = 0;
                }
            }

            // Заполняем итоговое поле нулями
            for (int i = 0; i < totalStates.Length; i++)
            {
                totalStates[i] = 0;
            }
        }

        private void UpdateButtonTexts()
        {
            for (int i = 0; i < buttonStates.GetLength(0); i++)
            {
                for (int j = 0; j < buttonStates.GetLength(1); j++)
                {
                    switch (buttonStates[i, j])
                    {
                        case 0:
                            buttons[i][j].Text = "";
                            break;
                        case 1:
                            buttons[i][j].Text = "x";
                            break;
                        case 2:
                            buttons[i][j].Text = "o";
                            break;
                    }
                }
            }

            for (int i = 0; i < totalStates.Length; i++)
            {
                switch (totalStates[i])
                {
                    case 0:
                        labels[i].Text = "";
                        break;
                    case 1:
                        labels[i].Text = "x";
                        break;
                    case 2:
                        labels[i].Text = "o";
                        break;
                }
            }
        }


        private void CreateField()
        {
            for (int i = 1; i <= 3; i++)
            {
                int absoluteCounter = 1;
                StackLayout FieldHr = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 15
                };
                for (int j = 1; j <= 3; j++)
                {
                    int Counter = 1;
                    List<Button> btns = new List<Button>();
                    StackLayout BigItem = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.Center,
                        StyleId = $"Cont_{absoluteCounter}"
                    };
                    for (int k = 1; k <= 3; k++)
                    {
                        StackLayout Item = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.Center,
                        };
                        for (int l = 1; l <= 3; l++)
                        {
                            Button button = new Button()
                            {
                                Text = "",
                                TextColor = Color.Black,
                                BackgroundColor = Color.WhiteSmoke,
                                StyleId = $"btn{Counter}{absoluteCounter}",
                                WidthRequest = 34,
                                HeightRequest = 34
                            };
                            button.Clicked += Button_Clicked;
                            btns.Add(button);
                            Counter++;

                            Item.Children.Add(button);
                        }

                        BigItem.Children.Add(Item);
                    }
                    absoluteCounter++;
                    buttons.Add(btns);

                    FieldHr.Children.Add(BigItem);
                }
                Field.Children.Add(FieldHr);
            }

            // Пробел
            BoxView rect = new BoxView()
            {
                WidthRequest = 150,
                HeightRequest = 40,
                BackgroundColor = Color.Transparent
            };
            Field.Children.Add(rect);

            // Создание итогового поля
            StackLayout TotalStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center
            };

            int c = 1;
            for (int i = 1; i <= 3; i++)
            {
                StackLayout st = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center
                };
                for (int j = 1; j <= 3; j++)
                {
                    Label elem = new Label()
                    {
                        Text = "",
                        StyleId = $"total{c}",
                        TextColor = Color.Black,
                        BackgroundColor = Color.WhiteSmoke,
                        WidthRequest = 45,
                        HeightRequest = 45
                    };
                    st.Children.Add(elem);
                    labels.Add(elem);
                    c++;
                }

                TotalStack.Children.Add(st);
            }

            Field.Children.Add(TotalStack);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}