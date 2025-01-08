using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Game1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultGamePage : ContentPage
    {
        // Определяем состояния кнопок: 0 - пусто, 1 - синий, 2 - красный
        private int[] buttonStates = new int[9];

        // Переменная хода [ true -> синий; false -> красный ]
        bool step = true;

        public DefaultGamePage()
        {
            InitializeComponent();
            InitializeButtonStates();
            UpdateButtonTexts();
            UpdateColorPlayer();
            BackgroundColor = Color.FromHex("#1a1a1a");
        }

        private void InitializeButtonStates()
        {
            for (int i = 0; i < buttonStates.Length; i++)
            {
                buttonStates[i] = 0; // Задаем пустое состояние для всех кнопок
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            // Получаем индекс кнопки на основе имени
            int buttonIndex = int.Parse(button.StyleId.Replace("btn", "")) - 1;

            // Изменяем состояние кнопки
            if (buttonStates[buttonIndex] == 0)
            {
                buttonStates[buttonIndex] = step ? 1 : 2;
                step = !step;
            }

            // Проверка на победу
            checkWin();

            // Обновляем текст кнопок согласно их состояниям
            UpdateButtonTexts();
        }

        private void UpdateColorPlayer()
        {
            if (step)
            {
                //BackgroundColor = Color.Blue;
                MainTitle.Text = "Ход синего";
            }
            else
            {
                //BackgroundColor = Color.Red;
                MainTitle.Text = "Ход красного";
            }
        }

        private void UpdateButtonTexts()
        {
            // Обновляем текст кнопок в соответствии с их состоянием
            for (int i = 0; i < buttonStates.Length; i++)
            {
                Button button = (Button)FindByName($"btn{i + 1}");
                switch (buttonStates[i])
                {
                    case 0:
                        button.Text = "";
                        break;
                    case 1:
                        button.Text = "x";
                        break;
                    case 2:
                        button.Text = "o";
                        break;
                }
            }

            // Обновляем информацию о дальнейшем ходе
            UpdateColorPlayer();
        }

        private void checkWin()
        {
            if (buttonStates[0] == 1 && buttonStates[1] == 1 && buttonStates[2] == 1 ||
                buttonStates[3] == 1 && buttonStates[4] == 1 && buttonStates[5] == 1 ||
                buttonStates[6] == 1 && buttonStates[7] == 1 && buttonStates[8] == 1 ||

                buttonStates[0] == 1 && buttonStates[4] == 1 && buttonStates[8] == 1 ||
                buttonStates[2] == 1 && buttonStates[4] == 1 && buttonStates[6] == 1 ||

                buttonStates[0] == 1 && buttonStates[3] == 1 && buttonStates[6] == 1 ||
                buttonStates[1] == 1 && buttonStates[4] == 1 && buttonStates[7] == 1 ||
                buttonStates[2] == 1 && buttonStates[5] == 1 && buttonStates[8] == 1)
            {
                SendPoupup("Победа синего");
                return;
            }
            if (buttonStates[0] == 2 && buttonStates[1] == 2 && buttonStates[2] == 2 ||
                buttonStates[3] == 2 && buttonStates[4] == 2 && buttonStates[5] == 2 ||
                buttonStates[6] == 2 && buttonStates[7] == 2 && buttonStates[8] == 2 ||

                buttonStates[0] == 2 && buttonStates[4] == 2 && buttonStates[8] == 2 ||
                buttonStates[2] == 2 && buttonStates[4] == 2 && buttonStates[6] == 2 ||

                buttonStates[0] == 2 && buttonStates[3] == 2 && buttonStates[6] == 2 ||
                buttonStates[1] == 2 && buttonStates[4] == 2 && buttonStates[7] == 2 ||
                buttonStates[2] == 2 && buttonStates[5] == 2 && buttonStates[8] == 2)
            {
                SendPoupup("Победа красного");
                return;
            }

            int count = 0;
            for (int i = 0; i < buttonStates.Length; i++) if (buttonStates[i] != 0) count++;
            if (count == 9)
            {
                SendPoupup("Ничья");
                return;
            }

        }

        private async void SendPoupup(string Message, int type = 0)
        {
            MainTitle.Text = Message;
            bool answer = await DisplayAlert("Игра закончена", Message + "\nХотите начать заново?", "Да", "Нет");
            if (answer) RestatGame();
        }

        private void RestatGame()
        {
            InitializeButtonStates();
            step = true;
            UpdateButtonTexts();
        }
    }
}