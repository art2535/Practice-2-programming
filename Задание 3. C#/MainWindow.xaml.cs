using System.Windows;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // конструктор по умолчанию
        public MainWindow()
        {
            InitializeComponent();
            MainLabel.Content = "Добро пожаловать в обучающее приложение\n\"Объектно-ориентированное программирование в С++\"";
            MainFrame.Navigate(new AuthorisationPage());
        }

        // метод для выхода из приложения
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Вопрос", 
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Close();
        }
    }
}
