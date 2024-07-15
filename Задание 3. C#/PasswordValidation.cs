using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Задание_3.C_
{
    // класс для валидации пароля
    public class PasswordValidation
    {
        // функция для хеширования пароля
        // принимает на вход пароль
        // возращает захешированный пароль
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // функция для верификации пароля
        // принмает на вход введенный пароль и хешированный пароль
        // возвращает логическое значения верификации пароля
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }

        // функция для валидации пароля
        // принимает на вход парль
        // возвращает логическое значение правильности ввода пароля
        public static bool Validation(string password)
        {
            var inputPassword = password;
            var hasNumber = new Regex(@"[0-9]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@"^.{6,12}$");
            var hasSymbols = new Regex(@"[!$%^&*()_=+{}\|/?.,]");
            if (!hasLowerChar.IsMatch(inputPassword))
            {
                MessageBox.Show("Пароль должен иметь хотя бы одну строчную букву", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!hasNumber.IsMatch(inputPassword))
            {
                MessageBox.Show("Пароль должен иметь хотя бы одну цифру", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!hasUpperChar.IsMatch(inputPassword))
            {
                MessageBox.Show("Пароль должен иметь хотя бы одну заглавную букву", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!hasMiniMaxChars.IsMatch(inputPassword))
            {
                MessageBox.Show("Пароль должен быть от 6 до 12 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!hasSymbols.IsMatch(inputPassword))
            {
                MessageBox.Show("Пароль должен иметь хотя бы один специальный символ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
