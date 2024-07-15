using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Xceed.Words.NET;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для Lesson5Page.xaml
    /// </summary>
    public partial class Lesson5Page : Page
    {
        // конструктор по умолчанию
        // косвенно вызывается функции загрузки Word-документа
        public Lesson5Page()
        {
            InitializeComponent();
            LoadWordDocument("Lessons\\Lesson5.docx");
        }

        // функция загрузки Word-документа с текстом и картинками
        private void LoadWordDocument(string filePath)
        {
            try
            {
                // Проверка наличия файла
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Файл {filePath} не найден.");
                    return;
                }

                // Загрузка документа Word
                var document = DocX.Load(filePath);

                // Создание нового FlowDocument для RichTextBox
                FlowDocument flowDocument = new FlowDocument();

                // Проход по каждому элементу документа
                foreach (var paragraph in document.Paragraphs)
                {
                    // Создание абзаца для текста
                    Paragraph p = new Paragraph(new Run(paragraph.Text))
                    {
                        Margin = new Thickness(0, 0, 0, 10),
                        FontSize = 18
                    };
                    flowDocument.Blocks.Add(p);

                    // Проверка на наличие картинок в абзаце
                    var pictures = paragraph.Pictures;
                    foreach (var picture in pictures)
                    {
                        using (var stream = picture.Stream)
                        {
                            // Сохранение изображения во временный поток
                            stream.Seek(0, SeekOrigin.Begin);

                            // Создание изображения и добавление его в абзац
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();

                            Image image = new Image
                            {
                                Source = bitmap,
                                Width = picture.Width,
                                Height = picture.Height,
                                Margin = new Thickness(0, 0, 0, 10)
                            };

                            BlockUIContainer container = new BlockUIContainer(image);
                            flowDocument.Blocks.Add(container);
                        }
                    }
                }

                // Установка созданного FlowDocument в RichTextBox
                DocumentTextBox.Document = flowDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки документа: {ex.Message}");
            }
        }

        // метод для перехода на окно меню
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        // метод для перехода на предыдущий урок
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Lesson4Page());
        }

        // метод для прохождения тренажера
        private void ButtonTraining_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Хотите начать тренажер?", "Предупреждение", MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                LessonTracker.CountLessons++;
                NavigationService.Navigate(new TrainingPage());
            }
        }
    }
}
