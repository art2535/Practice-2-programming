using System.Text.RegularExpressions;
using System.Windows;

namespace Задание_3.C_
{
    // класс для валидации Email
    public class EmailValidation
    {
        // метод для праильности ввода Email
        // принимает на вход Email при регистрации
        // возвращает логическое значение правильности ввода Email
        public static bool Validation(string email)
        {
            var regex = new Regex(@"^[a-zA-Z0-9._-]+@[a-z]+\.(ru|com)$");
            if (!regex.IsMatch(email))
            {
                MessageBox.Show("Неверный Email. Повторите ввод", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
