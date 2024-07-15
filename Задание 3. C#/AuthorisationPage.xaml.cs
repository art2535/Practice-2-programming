using OfficeOpenXml;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для AuthorisationPage.xaml
    /// </summary>
    public partial class AuthorisationPage : Page
    {
        // свойство для инициализации пользователя
        public static string UserName { get; private set; }

        // конструктор по умолчанию
        public AuthorisationPage()
        {
            InitializeComponent();
        }

        // метод для авторизации пользователя
        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserTextBox.Text;
            string password = PasswordTextBox.Password;
            UserName = userName;
            if (ValidateUser(userName, password))
            {
                MessageBox.Show("Вход в систему выполнен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new MenuPage());
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль. Повторите ввод!", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        // метод для перехода на окно регистрации
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }

        // метод для валидации пользователя
        // принимает на вход имя пользователя и пароль
        // возвращает логическое значение валидации пользователя
        private static bool ValidateUser(string username, string password)
        {
            FileInfo fileInfo = new FileInfo("База данных.xlsx");
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Предполагаем, что первый лист
                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++) // Предполагаем, что первая строка это заголовок
                {
                    string excelUsername = worksheet.Cells[row, 1].Text;
                    string storedHashedPassword = worksheet.Cells[row, 4].Text;
                    if (username.Equals(excelUsername, StringComparison.OrdinalIgnoreCase) &&
                        PasswordValidation.VerifyPassword(password, storedHashedPassword))
                        return true;
                }
            }
            return false;
        }
    }
}
