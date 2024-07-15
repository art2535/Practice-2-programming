using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для StatisticWindow.xaml
    /// </summary>
    public partial class StatisticWindow : Window
    {
        // поле для инициализации пользователя
        private string _user;

        // конструктор по умолчанию
        // принимает на вход количество пройденных уроков и результат тренажера
        // косвенно вызывается функция загрузки статистики по итоговому тесту
        public StatisticWindow(int completedLessons, int trainingResult)
        {
            InitializeComponent();
            _user = !string.IsNullOrEmpty(AuthorisationPage.UserName) ?
                AuthorisationPage.UserName : RegistrationPage.UserName;
            UserTextBlock.Text += _user;
            CountLessons.Text += completedLessons + "/5";
            TrainingResult.Text += trainingResult + "/5";
            LoadStatistic();
        }

        // метод закрытия окна
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // метод загрузки статистики из файла Excel
        private void LoadStatistic()
        {
            string username = AuthorisationPage.UserName;
            FileInfo fileInfo = new FileInfo("База данных.xlsx");
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Результаты теста");
                if (worksheet == null)
                {
                    NowTestResult.Text = "Текущий результат теста: Не найдено";
                    ItogTestResult.Text = "Лучший результат теста: Не найдено";
                    TheBestAttempt.Text = "Лучшая попытка: Не найдено";
                    return;
                }
                int rowCount = worksheet.Dimension.Rows;

                // Словарь для хранения информации о пользователях
                Dictionary<string, List<(int score, int attempt)>> userStatistics =
                    new Dictionary<string, List<(int score, int attempt)>>();

                // Пропускаем первую строку заголовков
                for (int row = 2; row <= rowCount; row++)
                {
                    string user = worksheet.Cells[row, 1].Text;
                    int score = int.Parse(worksheet.Cells[row, 2].Text);
                    if (userStatistics.ContainsKey(user))
                        userStatistics[user].Add((score, row - 1)); // Сохраняем попытку как (балл, номер строки)
                    else
                        userStatistics[user] = new List<(int score, int attempt)> { (score, row - 1) };
                }

                // Если искомый пользователь есть в словаре, обрабатываем его данные
                if (userStatistics.ContainsKey(username))
                {
                    var attempts = userStatistics[username];
                    int maxScore = attempts.Max(a => a.score);
                    int bestAttempt = attempts.FindIndex(a => a.score == maxScore) + 1; // Индекс первой попытки с макс баллом + 1
                    int lastScore = attempts.Last().score;

                    // Обновляем текстовые блоки
                    NowTestResult.Text = $"Текущий результат теста: {lastScore}/20";
                    ItogTestResult.Text = $"Лучший результат теста: {maxScore}/20";
                    TheBestAttempt.Text = $"Лучшая попытка: {bestAttempt}";
                }
                else
                {
                    // Если пользователь не найден, обнуляем текстовые блоки или выводим сообщение
                    NowTestResult.Text = "Текущий результат теста: Не найдено";
                    ItogTestResult.Text = "Лучший результат теста: Не найдено";
                    TheBestAttempt.Text = "Лучшая попытка: Не найдено";
                }
            }
        }
    }
}
