using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Linq;
using System.Windows;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        // поля для инициализации количесвта правильных ответов, общего количества вопросов
        // и инциализации пользователя
        private int _correctAnswers;
        private int _totalQuestions;
        private string User;

        // конструктор по умолчанию
        // принимает на вход количество правильных, неправильных ответов, общее количество вопросов
        public ResultsWindow(int correctAnswers, int incorrectAnswers, int totalQuestions)
        {
            InitializeComponent();

            string user = !string.IsNullOrEmpty(AuthorisationPage.UserName) 
                ? AuthorisationPage.UserName : RegistrationPage.UserName;
            UserTextBlock.Text += user;
            User = user;
            double percentage = ((double)correctAnswers / totalQuestions) * 100;
            int mark = CountMark(percentage);
            correctAnswersTextBlock.Text = $"Правильных ответов: {correctAnswers}";
            incorrectAnswersTextBlock.Text = $"Неправильных ответов: {incorrectAnswers}";
            totalQuestionsTextBlock.Text = $"Общее количество вопросов: {totalQuestions}";
            MarkTextBlock.Text = $"Оценка: {mark}";
            _correctAnswers = correctAnswers;
            _totalQuestions = totalQuestions;
        }

        // метод для закрытия окна
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null)
                mainWindow.MainFrame.Navigate(new MenuPage());
            this.Close();
        }

        // метод для сохраненния результатов в файл Excel
        // косвенно вызывается функция сохранения ответов в файл Excel
        private void ButtonSaveResults_Click(object sender, RoutedEventArgs e)
        {
            SaveAnswerForItogTesting(_correctAnswers, _totalQuestions);
        }

        // функция для вывода оценки за работу
        // принимает на вход процент выполненной работы
        // возращает оценку пользователя за тест
        private int CountMark(double percent)
        {
            if (percent < 50)
                return 2;
            else if (percent >= 50 && percent < 66)
                return 3;
            else if (percent >= 66 && percent <= 85)
                return 4;
            else
                return 5;
        }

        // метод для сохранения результатов тестирования в базу данных
        // принимает на вход текущий результат теста, общее количество вопросов в тесте
        private void SaveAnswerForItogTesting(int result, int total)
        {
            double percentForTesting = ((double)result / total) * 100;
            int mark = CountMark(percentForTesting);
            FileInfo fileInfo = new FileInfo("База данных.xlsx");
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet;
                int rowCount;
                if (fileInfo.Exists)
                {
                    worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Результаты теста") ??
                        package.Workbook.Worksheets.Add("Результаты теста");
                    worksheet.Cells[1, 1].Value = "Пользователь";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 2].Value = "Результат (баллов)";
                    worksheet.Cells[1, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 3].Value = "Максимальное количество баллов";
                    worksheet.Cells[1, 3].Style.Font.Bold = true;
                    worksheet.Cells[1, 4].Value = "Оценка";
                    worksheet.Cells[1, 4].Style.Font.Bold = true;
                    rowCount = worksheet.Dimension?.End.Row ?? 1;
                }
                else
                {
                    worksheet = package.Workbook.Worksheets.Add("Результаты теста");
                    worksheet.Cells[1, 1].Value = "Пользователь";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 2].Value = "Результат (баллов)";
                    worksheet.Cells[1, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 3].Value = "Максимальное количество баллов";
                    worksheet.Cells[1, 3].Style.Font.Bold = true;
                    worksheet.Cells[1, 4].Value = "Оценка";
                    worksheet.Cells[1, 4].Style.Font.Bold = true;
                    rowCount = 1;
                }
                worksheet.Cells[rowCount + 1, 1].Value = User;
                worksheet.Cells[rowCount + 1, 2].Value = result;
                worksheet.Cells[rowCount + 1, 3].Value = total;
                worksheet.Cells[rowCount + 1, 4].Value = mark;

                var tableRange = worksheet.Cells[1, 1, rowCount + 1, 4];
                tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                package.Save();
            }
            MessageBox.Show("Результаты сохранены в базу данных", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}