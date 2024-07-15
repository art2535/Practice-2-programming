using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        // конструктор по умолчанию
        public MenuPage()
        {
            InitializeComponent();
            string userName = !string.IsNullOrEmpty(AuthorisationPage.UserName)
                ? AuthorisationPage.UserName : RegistrationPage.UserName;
            User.Content = userName;
        }
        
        // метод для перехода на урок 1
        private void ButtonLesson1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Lesson1Page());
        }

        // метод для перехода на урок 2
        private void ButtonLesson2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Lesson2Page());
        }

        // метод для перехода на урок 3
        private void ButtonLesson3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Lesson3Page());
        }

        // метод для перехода на урок 4
        private void ButtonLesson4_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Lesson4Page());
        }

        // метод для перехода на урок 5
        private void ButtonLesson5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Lesson5Page());
        }

        // метод для перехода на тренажер
        private void ButtonTraining_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainingPage());
        }

        // метод для перехода на итоговый тест
        private void ButtonItogTesting_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ItogTestingPage());
        }

        // метод для выхода из аккаунта и перехода на окно авторизации
        private void ButtonExitFromAccaunt_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Хотите выйти из аккаунта?", "Предупреждение",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                NavigationService.Navigate(new AuthorisationPage());
        }

        // метод для отображения статистики
        private void ButtonStatistic_Click(object sender, RoutedEventArgs e)
        {
            StatisticWindow statisticWindow = new StatisticWindow(LessonTracker.CountLessons, TrainingPage.CorrectAnswers);
            statisticWindow.ShowDialog();
        }
    }
}
