using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Задание_3.C_
{
    /// <summary>
    /// Логика взаимодействия для TrainingPage.xaml
    /// </summary>
    public partial class TrainingPage : Page
    {
        // поля для инициализации параметров, необходимых для прохождения тренажера
        private Random _random;
        private List<string> _programs;
        private int _correctAnswer;
        private int _correctAnswerCount;
        private int _incorrectAnswerCount;
        private int _totalQuestions;

        // поля для инициализации переменных программ и дальнейшего подсчета правильного ответа
        private int _width, _lenght, _radius, _side, _baseLength, _height, _baseLength1, _height1;

        // свойтсво для передачи количества правильных ответов на страницу результатов тренажера
        public static int CorrectAnswers {  get; set; } = 0;

        // конструктор по умолчанию
        // косвенно вызываются функции генерации программ, их перемешивания,
        // генерации чисел для инициализации переменных для программ и вывод
        // на экран в случайном порядке
        public TrainingPage()
        {
            InitializeComponent();
            _random = new Random();
            _programs = GeneratePrograms();
            ShufflePrograms();
            DisplayNextProgram();
        }

        // функция для генерации программ
        // возвращает список программ
        private List<string> GeneratePrograms()
        {
            var programs = new List<string>
            {
                GenerateProgram1(),
                GenerateProgram2(),
                GenerateProgram3(),
                GenerateProgram4(),
                GenerateProgram5()
            };
            _totalQuestions = programs.Count;
            return programs;
        }

        // функция для перемешивания программ
        private void ShufflePrograms()
        {
            _programs = _programs.OrderBy(p => _random.Next()).ToList();
        }

        // функция для отображения программ
        private void DisplayNextProgram()
        {
            if (_programs.Count > 0)
            {
                var program = _programs.First();
                _programs.RemoveAt(0);
                ProgramTextBox.Text = program;
                _correctAnswer = CalculateAnswerFromProgram(program);
            }
            else
            {
                ButtonResult_Click(null, null);
                return;
            }
        }

        // метод для подсыета правильного ответа программы
        // принимает на вход текст программы, сверяет их с данными
        // возращает результат программы
        private int CalculateAnswerFromProgram(string programText)
        {
            // Парсим текст программы и вычисляем правильный ответ
            // Пример демонстрации вычисления для программы 1
            if (programText.Contains("class Rectangle"))
                return _lenght * _width;
            if (programText.Contains("class Circle"))
                return (int)(3.14 * _radius * _radius);
            if (programText.Contains("class Square"))
                return _side * _side;
            if (programText.Contains("class Triangle"))
                return (int)(0.5 * _baseLength * _height);
            if (programText.Contains("class Parallelogram"))
                return _baseLength1 * _height1;
            return 0;
        }

        // метод для проверки ответа
        private void CheckAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AnswerTextBox.Text, out int userAnswer))
            {
                if (userAnswer == _correctAnswer)
                {
                    _correctAnswerCount++;
                    MessageBox.Show("Ответ правильный!");
                }
                else
                {
                    _incorrectAnswerCount++;
                    MessageBox.Show($"Ответ неправильный. Правильный ответ: {_correctAnswer}");
                }
                AnswerTextBox.Text = "";
                DisplayNextProgram();
            }
            else
                MessageBox.Show("Введите целое число!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // метод для завершения тестирования
        private void ButtonStopTraining_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Завершить тест?", "Вопрос", MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
                NavigationService.Navigate(new MenuPage());
        }

        // функции для генрации чисел для программ
        // возращает текст программы с инициализированными переменными
        // значения для переменных присваиваются полям класса
        private string GenerateProgram1()
        {
            _lenght = _random.Next(1, 101);
            _width = _random.Next(1, 101);
            return $@"#include <iostream>
using namespace std;

class Shape {{
public:
    virtual int area() = 0;
}};

class Rectangle : public Shape {{
private:
    int length;
    int width;
public:
    Rectangle(int l, int w) : length(l), width(w) {{}}
    int area() override {{
        return length * width;
    }}
}};

int main() {{
    Rectangle rect({_lenght}, {_width});
    cout << ""Площадь прямоугольника: "" << rect.area() << endl;
    return 0;
}}";
        }

        private string GenerateProgram2()
        {
            _radius = _random.Next(1, 51);
            return $@"#include <iostream>
using namespace std;

class Shape {{
public:
    virtual int area() = 0;
}};

class Circle : public Shape {{
private:
    int radius;
public:
    Circle(int r) : radius(r) {{}}
    int area() override {{
        return 3.14 * radius * radius;
    }}
}};

int main() {{
    Circle circle({_radius});
    cout << ""Площадь круга: "" << circle.area() << endl;
    return 0;
}}";
        }

        private string GenerateProgram3()
        {
            _side = _random.Next(1, 101);
            return $@"#include <iostream>
using namespace std;

class Shape {{
public:
    virtual int area() = 0;
}};

class Square : public Shape {{
private:
    int side;
public:
    Square(int s) : side(s) {{}}
    int area() override {{
        return side * side;
    }}
}};

int main() {{
    Square square({_side});
    cout << ""Площадь квадрата: "" << square.area() << endl;
    return 0;
}}";
        }

        private string GenerateProgram4()
        {
            _baseLength = _random.Next(1, 101);
            _height = _random.Next(1, 101);
            return $@"#include <iostream>
using namespace std;

class Shape {{
public:
    virtual int area() = 0;
}};

class Triangle : public Shape {{
private:
    int base;
    int height;
public:
    Triangle(int b, int h) : base(b), height(h) {{}}
    int area() override {{
        return 0.5 * base * height;
    }}
}};

int main() {{
    Triangle triangle({_baseLength}, {_height});
    cout << ""Площадь треугольника: "" << triangle.area() << endl;
    return 0;
}}";
        }

        private string GenerateProgram5()
        {
            _baseLength1 = _random.Next(1, 101);
            _height1 = _random.Next(1, 101);
            return $@"#include <iostream>
using namespace std;

class Shape {{
public:
    virtual int area() = 0;
}};

class Parallelogram : public Shape {{
private:
    int base;
    int height;
public:
    Parallelogram(int b, int h) : base(b), height(h) {{}}
    int area() override {{
        return base * height;
    }}
}};

int main() {{
    Parallelogram parallelogram({_baseLength1}, {_height1});
    cout << ""Площадь параллелограмма: "" << parallelogram.area() << endl;
    return 0;
}}";
        }

        // метод для отображения результатов тестирования
        private void ButtonResult_Click(object sender, RoutedEventArgs e)
        {
            CorrectAnswers = _correctAnswerCount;
            ResultsWindow resultsWindow = new ResultsWindow(_correctAnswerCount, _incorrectAnswerCount, _totalQuestions);
            resultsWindow.Show();
        }

        // метод для перехода на окно итогового теста
        private void ItogTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Начать итоговый тест?", "Вопрос", MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
                NavigationService.Navigate(new ItogTestingPage());
        }
    }
}
