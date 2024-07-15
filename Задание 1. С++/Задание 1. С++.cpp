/* Вариант 11. Скопируйте текст раздела III. «Чтение файлов» без заголовка
в файл TASK_1_NAME.txt (вместо NAME впишите свою фамилию на латинице). Напишите скрипт (код), выполняющий
поиск всех трёхбуквенных слов (не зависимо от части речи и языка) и выводящий их общее число
и номера строк, где они встречаются */

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <locale>
using namespace std;
// Функция для проверки, является ли символ буквой русского или английского алфавита
bool isAlphabetic(char c)
{
    return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||
        (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') || (c == 'ё') || (c == 'Ё');
}
// Функция для проверки, является ли символ пробелом, точкой, запятой или переносом строки
bool isWordBoundary(char c)
{
    return c == ' ' || c == '.' || c == ',' || c == '\n';
}
int main()
{
    setlocale(LC_ALL, "Russian");
    ifstream file("TASK_1_PETROV.txt");
    // Проверка открытия файла
    if (!file.is_open())
    {
        cout << "Ошибка открытия файла." << endl;
        return -1;
    }
    file.seekg(0, ios::end);
    // Проверка содержимого файла на пустоту
    if (file.tellg() == 0)
    {
        cout << "Файл пуст" << endl;
        return -1;
    }
    file.seekg(0, ios::beg);
    string line;
    vector<int> wordLines; // Вектор, хранящий номера строк
    int totalWords = 0;
    int lineNumber = 0;
    // пока не конец файла
    while (getline(file, line))
    {
        lineNumber++;
        int wordStart = -1;
        for (int i = 0; i < line.length(); i++)
        {
            // Проверка, что символ является буквой русского или английского алфавита
            if (isAlphabetic(line[i]))
            {
                if (wordStart == -1)
                    wordStart = i;
            }
            else
            {
                if (wordStart != -1)
                {
                    // Подсчет длины слова
                    int wordLength = i - wordStart;
                    if (wordLength == 3)
                    {
                        // Условие 1: Слово окружено пробелами
                        if ((wordStart > 0 && line[wordStart - 1] == ' ') && (line[i] == ' '))
                        {
                            wordLines.push_back(lineNumber);
                            totalWords++;
                        }
                        // Условие 2: Слово начинается с пробела и заканчивается символом новой строки
                        else if ((wordStart > 0 && line[wordStart - 1] == ' ') && (line[i] == '\n' || i == line.length()))
                        {
                            wordLines.push_back(lineNumber);
                            totalWords++;
                        }
                        // Условие 3: Слово начинается с пробела и заканчивается запятой или точкой
                        else if ((wordStart > 0 && line[wordStart - 1] == ' ') && (line[i] == '.' || line[i] == ','))
                        {
                            wordLines.push_back(lineNumber);
                            totalWords++;
                        }
                        // Условие 4: Слово находится в начале строки и заканчивается пробелом, запятой, точкой и т.д.
                        else if (wordStart == 0 && isWordBoundary(line[i]))
                        {
                            wordLines.push_back(lineNumber);
                            totalWords++;
                        }
                    }
                    // Обновление индекса
                    wordStart = -1;
                }
            }
        }
        // Проверка последнего слова в строке, если строка закончилась не на пробеле или знаке пунктуации
        if (wordStart != -1)
        {
            int wordLength = line.length() - wordStart;
            if (wordLength == 3)
            {
                // Условие 4: Слово находится в начале строки и заканчивается пробелом, запятой, точкой и т.д.
                if (wordStart == 0)
                {
                    wordLines.push_back(lineNumber);
                    totalWords++;
                }
                // Условие 2: Слово начинается с пробела и заканчивается символом новой строки
                else if (line[wordStart - 1] == ' ' && (line[line.length() - 1] == '\n' || line.length() == wordStart + 3))
                {
                    wordLines.push_back(lineNumber);
                    totalWords++;
                }
            }
        }
    }
    // Закрытие файла
    file.close();
    // Вывод информации на экран
    cout << "Общее число трехбуквенных слов: " << totalWords << endl;
    cout << "Номера строк, где они встречаются:" << endl;
    for (int i = 0; i < wordLines.size(); i++)
        cout << wordLines[i] << " ";
    return 0;
}