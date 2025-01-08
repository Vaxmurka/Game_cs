using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Game1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProGamePage : ContentPage
    {
        // Определяем состояния кнопок: 0 -> пусто
        //                              1 ... n -> индексы X (Синие)
        //                              -n ... -1 -> индексы O (Красные)
        private int[] buttonStates = new int[9];

        // Массивы с кнопками
        List<Button> buttons = new List<Button>();
        List<Button> X_buttons = new List<Button>();
        List<Button> O_buttons= new List<Button>();

        // Количество индексов
        int count;

        // Контейнер для уже используемых(запретных) индексов
        private List<int> UsedIndex = new List<int>();

        // Переменная используемого индекса
        int currentIndex;

        // Переменная хода [ true -> синий; false -> красный ]
        bool step = true;

        // Состояние кнопок поля [ true -> разрешено; false -> запрещено ]
        bool toogleEnable = false;

        public ProGamePage()
        {
            InitializeComponent();
            InitializeButtonStates();
            BackgroundColor = Color.FromHex("#1a1a1a");
        }

        private void InitializeButtonStates()
        {
            // Заполняем список кнопок
            for (int i = 1; i <= 9; i++)
            {
                Button button = (Button)FindByName($"btn{i}");
                if (button != null)
                {
                    buttons.Add(button);
                    button.TextColor = Color.Black;
                }
                    
            }

            // Задаем пустое состояние для всех кнопок
            for (int i = 0; i < buttonStates.Length; i++)
            {
                buttonStates[i] = 0; 
            }
            toogleEnable = false;
        }

        // Меняет информацию при каждом ходе
        private void UpdateColorPlayer()
        {
            if (step)
            {
                MainTitle.Text = "Ход синего";
                setEnabled('x', true);
                toogleEnable = false;
            }
            else
            {
                MainTitle.Text = "Ход красного";
                setEnabled('o', true);
                toogleEnable = false;
            }
            setStateXO_Buttons();
        }

        // Функция изменения состояний на нажатие кнопок
        // (char s [ 'x', 'o', 'f' -> Field ]; bool state [ true -> enable; false -> disable ])
        private void setEnabled(char s, bool state)
        {
            if (s == 'x')
            {
                foreach (Button button in X_buttons)
                    button.IsEnabled = state;
                foreach (Button button in O_buttons)
                    button.IsEnabled = !state;
            }
            else if (s == 'o')
            {
                foreach (Button button in X_buttons)
                    button.IsEnabled = !state;
                foreach (Button button in O_buttons)
                    button.IsEnabled = state;
            }
            // Если не выбран индекс хода, то запрещать нажатие на поле
            // Запрещат нажатие на индексы противника
        }

        private void UpdateButtonTexts()
        {
            // Обновляем текст кнопок в соответствии с их состоянием
            for (int i = 0; i < buttonStates.Length; i++)
            {
                //Button button = (Button)FindByName($"btn{i + 1}");

                if (buttonStates[i] != 0)
                {
                    if (buttonStates[i] > 0)
                        buttons[i].Text = "x" + buttonStates[i].ToString();
                    else
                        buttons[i].Text = "o" + (-buttonStates[i]).ToString();
                }
                else
                    buttons[i].Text = "";
            }

            // Обновляем информацию о дальнейшем ходе
            UpdateColorPlayer();
        }

        // Создание кнопок с индексами
        private void CreateBtn_Clicked(object sender, EventArgs e)
        {
            // Очищаем предыдущие кнопки (если есть)
            Field_X.Children.Clear();
            Field_O.Children.Clear();

            // Создаем и добавляем кнопки для X
            for (int i = 0; i < count; i++)
            {
                Button btn = new Button
                {
                    Text = $"X{i + 1}",
                    TextColor = Color.Black,
                    BackgroundColor = Color.WhiteSmoke,
                    StyleId = $"X{i + 1}",
                    WidthRequest = 50
                };
                btn.Clicked += X_btn_Clicked;
                Field_X.Children.Add(btn);
                X_buttons.Add(btn);
            }

            // Создаем и добавляем кнопки для O
            for (int i = 0; i < count; i++)
            {
                Button btn = new Button
                {
                    Text = $"O{i + 1}",
                    TextColor = Color.Black,
                    BackgroundColor = Color.WhiteSmoke,
                    StyleId = $"O{i + 1}",
                    WidthRequest = 50
                };
                btn.Clicked += O_btn_Clicked;
                Field_O.Children.Add(btn);
                O_buttons.Add(btn);
            }
            stepper.IsVisible = false;
            CreateBtn.IsVisible = false;

            UpdateColorPlayer();
        }

        private void setStateXO_Buttons()
        {
            foreach (Button btn in X_buttons)
            {
                if (step)
                {
                    bool flag = UsedIndex.Contains(int.Parse(btn.Text.Replace("X", "")));
                    Debug.Write(flag);

                    btn.BorderColor = flag ? Color.Red : Color.White;
                    btn.TextColor = flag ? Color.Gray : Color.Black;
                    btn.IsEnabled = !flag;
                }
                else
                    btn.IsEnabled = false;
                
            }

            foreach (Button btn in O_buttons)
            {
                if (!step)
                {
                    bool flag = UsedIndex.Contains(-int.Parse(btn.Text.Replace("O", "")));
                    Debug.Write(flag);

                    btn.BorderColor = flag ? Color.Red : Color.White;
                    btn.TextColor = flag ? Color.Gray : Color.Black;
                    btn.IsEnabled = !flag;
                }
                else
                    btn.IsEnabled = false;
                
            }
        }

        private void X_btn_Clicked(object sender, EventArgs e)
        {
            Button ChoiseButton = (Button)sender;
            setStateXO_Buttons();

            ChoiseButton.BorderColor = Color.Green;
            ChoiseButton.TextColor = Color.Green;

            // Получаем индекс конопки на основе имени
            int choiseIndex = int.Parse(ChoiseButton.StyleId.Replace("X", ""));

            if (step) currentIndex = choiseIndex;
            else currentIndex = 0;

            // Разрешение на нажатие на поле
            if (currentIndex != 0)
                toogleEnable = true;
        }

        private void O_btn_Clicked(object sender, EventArgs e)
        {
            Button ChoiseButton = (Button)sender;
            setStateXO_Buttons();

            ChoiseButton.BorderColor = Color.Green;
            ChoiseButton.TextColor = Color.Green;

            // Получаем индекс конопки на основе имени
            int choiseIndex = int.Parse(ChoiseButton.StyleId.Replace("O", ""));

            if (!step) currentIndex = -choiseIndex;

            // Разрешение на нажатие на поле
            if (currentIndex != 0)
                toogleEnable = true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!toogleEnable) return;

            Button button = (Button)sender;

            // Получаем индекс кнопки на основе имени
            int buttonIndex = int.Parse(button.StyleId.Replace("btn", "")) - 1;

            // Изменяем состояние кнопки
            if (buttonStates[buttonIndex] == 0 || Math.Abs(buttonStates[buttonIndex]) < Math.Abs(currentIndex))
            {
                buttonStates[buttonIndex] = currentIndex;
                step = !step;
            }
            else return;

            // Добавляем использованный индекс в массив 
            UsedIndex.Add(currentIndex);
            setStateXO_Buttons();

            // Проверка на победу
            checkWin();

            // Обновляем текст кнопок согласно их состояниям
            UpdateButtonTexts();
        }

        private void checkWin()
        {
            // Создаем массив состоящий из реальных состояний (без индексов) [ 1 -> Синие; -1 -> Красные ]
            int[] array = new int[9];
            for (int i = 0; i < buttonStates.Length; ++i)
            {
                if (buttonStates[i] == 0) array[i] = 0;
                else if (buttonStates[i] > 0) array[i] = 1;
                else array[i] = -1;
            }


            if (array[0] == 1 && array[1] == 1 && array[2] == 1 ||
                array[3] == 1 && array[4] == 1 && array[5] == 1 ||
                array[6] == 1 && array[7] == 1 && array[8] == 1 ||

                array[0] == 1 && array[4] == 1 && array[8] == 1 ||
                array[2] == 1 && array[4] == 1 && array[6] == 1 ||

                array[0] == 1 && array[3] == 1 && array[6] == 1 ||
                array[1] == 1 && array[4] == 1 && array[7] == 1 ||
                array[2] == 1 && array[5] == 1 && array[8] == 1)
            {
                SendPoupup("Победа синего");
                return;
            }
            if (array[0] == -1 && array[1] == -1 && array[2] == -1 ||
                array[3] == -1 && array[4] == -1 && array[5] == -1 ||
                array[6] == -1 && array[7] == -1 && array[8] == -1 ||

                array[0] == -1 && array[4] == -1 && array[8] == -1 ||
                array[2] == -1 && array[4] == -1 && array[6] == -1 ||

                array[0] == -1 && array[3] == -1 && array[6] == -1 ||
                array[1] == -1 && array[4] == -1 && array[7] == -1 ||
                array[2] == -1 && array[5] == -1 && array[8] == -1)
            {
                SendPoupup("Победа красного");
                return;
            }

            for (int i = 0; i < array.Length; ++i) if (array[i] == 0) return;
            SendPoupup("Ничья");
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
            clearFields();
        }

        private void clearFields()
        {
            Field_X.Children.Clear();
            Field_O.Children.Clear();
            stepper.Value = 5;
            stepper.IsVisible = true;
            CreateBtn.IsVisible = true;

            X_buttons.Clear();
            O_buttons.Clear();
            UsedIndex.Clear();
        }

        private void setCountIndex(object sender, ValueChangedEventArgs e)
        {
            CounterIndex.Text = String.Format("Количество переменных: {0:F0}", e.NewValue);
            count = (int)e.NewValue;
        }
    }
}