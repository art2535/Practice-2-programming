using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для ItogTestingPage.xaml
    /// </summary>
    public partial class ItogTestingPage : Page
    {
        // поля для инициализации параметров, необходимых для прохождения итогового теста
        private Random _random;
        private List<Question> _questions;
        private int _currentQuestionIndex;
        private bool _isFirstClick = true;
        private string User;

        // констуктор по умолчанию
        // косвенно вывзваются функции загрузки вопросов из файла txt,
        // перемешивания вопрсов и их вывод в случайном порядке
        public ItogTestingPage()
        {
            InitializeComponent();
            string user = !string.IsNullOrEmpty(AuthorisationPage.UserName) ?
                AuthorisationPage.UserName : RegistrationPage.UserName;
            User = user;
            _random = new Random();
            LoadQuestions("Вопросы к тесту.txt");
            ShuffleAndSelectQuestions();
            DisplayNextQuestion();
        }

        // метод загрузки вопросов из файла txt
        private void LoadQuestions(string filePath)
        {
            _questions = new List<Question>();
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Файл с вопросами не найден: " + filePath);
                    return;
                }
                var lines = File.ReadAllLines(filePath);
                Question currentQuestion = null;
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    if (line.StartsWith("Q:"))
                    {
                        if (currentQuestion != null)
                            _questions.Add(currentQuestion);
                        currentQuestion = new Question
                        {
                            Text = line.Substring(2).Trim(),
                            Options = new List<string>()
                        };
                    }
                    else if (line.StartsWith("A:") && currentQuestion != null)
                    {
                        var option = line.Substring(2).Trim();
                        if (option.EndsWith("(Правильный)"))
                        {
                            option = option.Replace("(Правильный)", "").Trim();
                            currentQuestion.CorrectOption = option;
                        }
                        currentQuestion.Options.Add(option);
                    }
                }
                if (currentQuestion != null)
                    _questions.Add(currentQuestion);
                if (!_questions.Any())
                    MessageBox.Show("Файл с вопросами не содержит вопросов.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении файла с вопросами: " + ex.Message);
            }
        }

        // функция для перемешивания вопросов и вывод в случаном порядке
        private void ShuffleAndSelectQuestions()
        {
            if (_questions == null || !_questions.Any())
            {
                MessageBox.Show("Нет доступных вопросов.");
                return;
            }
            // Перемешивание вопросов и выбор первых 20
            _questions = _questions.OrderBy(q => _random.Next()).Take(20).ToList();
            _currentQuestionIndex = 0;
        }

        // метод для записи ответа на вопрос в баз данных
        // косвенно вызываются функции сохранения ответов и показ следующего вопроса
        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedOption = clickedButton.Content.ToString();
            SaveAnswer(QuestionTextBlock.Text, selectedOption);
            DisplayNextQuestion(); // Показать следующий вопрос
        }

        // функция для показза следующего вопроса
        private void DisplayNextQuestion()
        {
            if (_currentQuestionIndex >= _questions.Count)
            {
                ButtonResult_Click(null, null);
                return;
            }
            var currentQuestion = _questions[_currentQuestionIndex];
            // Отображение текста вопроса
            QuestionTextBlock.Text = $"Вопрос {_currentQuestionIndex + 1}: {currentQuestion.Text}";
            // Перемешивание вариантов ответов
            var shuffledOptions = currentQuestion.Options.OrderBy(o => _random.Next()).ToList();
            // Назначение текста кнопкам
            if (shuffledOptions.Count >= 3)
            {
                OptionAButton.Content = shuffledOptions[0];
                OptionBButton.Content = shuffledOptions[1];
                OptionCButton.Content = shuffledOptions[2];
            }
            _currentQuestionIndex++;
        }

        // метод для сохранения ответа на вопрос в бвзу данных
        // на вход подаются текст вопроса и выбранный пользователем вариант ответа
        private void SaveAnswer(string question, string selectedOption)
        {
            FileInfo fileInfo = new FileInfo("База данных.xlsx");
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet;
                int rowCount;

                if (fileInfo.Exists)
                {
                    // Удаляем лист при первом нажатии
                    if (_isFirstClick)
                    {
                        worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == $"Итоговый тест {User}");
                        if (worksheet != null)
                            package.Workbook.Worksheets.Delete(worksheet);
                        _isFirstClick = false; // Устанавливаем флаг, что первый клик был
                    }

                    // Добавляем данные в существующий лист при последующих нажатиях
                    worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == $"Итоговый тест {User}") ??
                        package.Workbook.Worksheets.Add($"Итоговый тест {User}");
                    worksheet.Cells[1, 1].Value = "Вопрос";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 2].Value = "Ответ";
                    worksheet.Cells[1, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 3].Value = "Правильный ответ";
                    worksheet.Cells[1, 3].Style.Font.Bold = true;
                    rowCount = worksheet.Dimension?.End.Row ?? 1;
                }
                else
                {
                    // Создаем новый лист, если файл не существует
                    worksheet = package.Workbook.Worksheets.Add($"Итоговый тест {User}");
                    worksheet.Cells[1, 1].Value = "Вопрос";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 2].Value = "Ответ";
                    worksheet.Cells[1, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 3].Value = "Правильный ответ";
                    worksheet.Cells[1, 3].Style.Font.Bold = true;
                    rowCount = 1;
                }

                // Добавляем данные в таблицу
                worksheet.Cells[rowCount + 1, 1].Value = question;
                worksheet.Cells[rowCount + 1, 2].Value = selectedOption;
                var currentQuestion = _questions[_currentQuestionIndex - 1];
                worksheet.Cells[rowCount + 1, 3].Value = currentQuestion.CorrectOption;

                // Применяем стили к таблице
                var tableRange = worksheet.Cells[1, 1, rowCount + 1, 3];
                tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Автоматически подгоняем ширину столбцов
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Сохраняем изменения в файле Excel
                package.Save();
            }
        }

        // метод для сохранения результатов теста в базу данных
        private void ButtonResult_Click(object sender, RoutedEventArgs e)
        {
            int correctAnswers = 0;
            int incorrectAnswers = 0;
            using (ExcelPackage package = new ExcelPackage(new FileInfo("База данных.xlsx")))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == $"Итоговый тест {User}");
                if (worksheet != null)
                {
                    for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                    {
                        string question = worksheet.Cells[i, 1].Value?.ToString();
                        string selectedOption = worksheet.Cells[i, 2].Value?.ToString();
                        string correctOption = worksheet.Cells[i, 3].Value?.ToString();
                        if (selectedOption == correctOption)
                            correctAnswers++;
                        else
                            incorrectAnswers++;
                    }
                }
            }
            ResultsWindow resultsWindow = new ResultsWindow(correctAnswers, incorrectAnswers, _questions.Count);
            resultsWindow.Show();
        }

        // метод для завершения теста
        private void ButtonCompleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Завершить тест?", "Вопрос", MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
                NavigationService.Navigate(new MenuPage());
        }
    }

    // класс, необходимый для оторажения вопросов на экран и записи ответа на вопрос
    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string CorrectOption { get; set; }
    }
}