#include <filesystem>
#include <fstream>
#include <iostream>
namespace fs = std::filesystem;
using namespace std;
int main()
{
    setlocale(LC_ALL, "Rus");
    try
    {
        // Абсолютные пути к исходному и целевому каталогам
        string sourceDir = "D:\\Visual Studio 2022. Практика\\УП.01. Петров Артем. С224\\Задание 1. С++";
        string targetDir = "D:\\Visual Studio 2022. Практика\\УП.01. Петров Артем. С224\\Задание 2. С++";

        // Путь к файлу TASK_1_PETROV.txt в исходном каталоге
        string taskFilePath = sourceDir + "\\TASK_1_PETROV.txt";

        // Создание подкаталогов в целевом каталоге DIR_1\DIR_2
        string dir1Path = targetDir + "\\DIR_1";
        string dir2Path = dir1Path + "\\DIR_2";
        fs::create_directories(dir2Path);

        // Вывод информации на экран
        cout << "Каталоги DIR_1/DIR_2 созданы:" << endl;
        for (const auto& entry : fs::directory_iterator(dir1Path))
            cout << entry.path().string() << endl;
        cout << endl;

        // Перемещение файла TASK_1_PETROV.txt в каталог DIR_2 и запись нового пути в ONE_NEW_PATH.txt
        string newFilePath = dir2Path + "\\" + fs::path(taskFilePath).filename().string();
        fs::rename(taskFilePath, newFilePath);

        // Вывод информации на экран
        cout << "Файл TASK_1_PETROV.txt перемещен в каталог DIR_2:" << endl;
        for (const auto& entry : fs::directory_iterator(dir2Path))
            cout << entry.path().string() << endl;
        cout << endl;

        // Запись нового пути в файл ONE_NEW_PATH.txt в DIR_2
        ofstream newFilePathFile(dir2Path + "\\ONE_NEW_PATH.txt");

        // Вывод информации на экран
        cout << "Файл ONE_NEW_PATH.txt создан:" << endl;
        for (const auto& entry : fs::directory_iterator(dir2Path))
            cout << entry.path().string() << endl;
        cout << endl;
        if (newFilePathFile.is_open())
        {
            newFilePathFile << newFilePath;
            cout << "Содержимое файла ONE_NEW_PATH.txt:" << endl;
            cout << newFilePath << endl;
            newFilePathFile.close();
        }
        else
        {
            cout << "Невозможно открыть ONE_NEW_PATH.txt для записи!" << endl;
            return -1;
        }
        cout << endl;
        // Копирование файла ONE_NEW_PATH.txt из DIR_2 в DIR_1
        fs::copy(dir2Path + "\\ONE_NEW_PATH.txt", dir1Path + "\\ONE_NEW_PATH.txt");

        // Вывод информации на экран
        cout << "Файл ONE_NEW_PATH.txt скопирован из каталога DIR_2 в каталог DIR_1:" << endl;
        for (const auto& entry : fs::directory_iterator(dir1Path))
            cout << entry.path().string() << endl;
        cout << endl;

        // Удаление DIR_2 вместе с содержимым
        fs::remove_all(dir2Path);
        fs::remove(dir2Path);

        // Вывод информации на экран
        cout << "Содержимое каталога D:/.../Задание 2. С++/DIR_1 после удаления каталога DIR_2:" << endl;
        for (const auto& entry : fs::directory_iterator(dir1Path))
            cout << entry.path().string() << endl;
        cout << endl;

        // Вывод содержимого каталога DIR_1
        cout << "Содержимое каталога DIR_1:" << endl;
        for (const auto& entry : fs::directory_iterator(dir1Path))
            cout << entry.path().string() << endl;
    }
    // обработка исключений
    catch (const fs::filesystem_error& e)
    {
        cerr << "Ошибка файловой системы: " << e.what() << endl;
        return -1;
    }
    catch (const exception& e)
    {
        cerr << "Общая ошибка: " << e.what() << endl;
        return -1;
    }
    catch (...)
    {
        cerr << "Неизвестная ошибка!" << endl;
        return -1;
    }
    return 0;
}