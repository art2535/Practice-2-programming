using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        // свойство для инициализации пользователя
        public static string UserName { get; private set; }

        // конструктор по умолчанию
        public RegistrationPage()
        {
            InitializeComponent();
        }

        // метод для перехода на окно авторизации
        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorisationPage());
        }

        // метод для проверки правильности ввода данных и дальнейшей регистрацией
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            var hasMinMaxSymbols = new Regex(@"^.{5,10}$");
            string userName = UserTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Password;
            string passwordRepeat = PasswordRepeatTextBox.Password;
            UserName = userName;
            FileInfo fileInfo = new FileInfo("База данных.xlsx");
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordRepeat))
            {
                MessageBox.Show("Поля не должны быть пустыми", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!hasMinMaxSymbols.IsMatch(userName))
            {
                MessageBox.Show("Имя пользователя должно быть от 5 до 16 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!PasswordValidation.Validation(password))
                return;
            if (!EmailValidation.Validation(email))
                return;
            if (password != passwordRepeat)
            {
                MessageBox.Show("Пароли не совпадают. Повторите ввод", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string hashedPassword = PasswordValidation.HashPassword(password);

            // запись данных в базу данных
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet;
                int rowCount;
                if (fileInfo.Exists)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    rowCount = worksheet.Dimension.End.Row;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        string existingLogin = worksheet.Cells[row, 1].Text;
                        string existingEmail = worksheet.Cells[row, 2].Text;
                        if (userName.Equals(existingLogin, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Пользователь с таким именем уже зарегистрирован", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else if (email.Equals(existingEmail, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Пользователь с таким Email уже зарегистрирован", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
                else
                {
                    worksheet = package.Workbook.Worksheets.Add("Регистрация");
                    worksheet.Cells[1, 1].Value = "Имя пользователя";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 2].Value = "Email";
                    worksheet.Cells[1, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 3].Value = "Пароль";
                    worksheet.Cells[1, 3].Style.Font.Bold = true;
                    worksheet.Cells[1, 4].Value = "Хешированный пароль";
                    worksheet.Cells[1, 4].Style.Font.Bold = true;
                    rowCount = 1;
                }
                worksheet.Cells[rowCount + 1, 1].Value = userName;
                worksheet.Cells[rowCount + 1, 2].Value = email;
                worksheet.Cells[rowCount + 1, 3].Value = password;
                worksheet.Cells[rowCount + 1, 4].Value = hashedPassword;

                // Установка границ для всей таблицы
                var tableRange = worksheet.Cells[1, 1, rowCount + 1, 4];
                tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Автоматическое изменение ширины столбцов
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Сохранение файла
                package.Save();
            }
            MessageBox.Show("Вы авторизованы в системе", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new MenuPage());
        }
    }
}