using System;
using Words.Mechanics;

namespace Words
{
    class Program
    {
        enum MainMenuActions
        {
            SetCustomSettings = 1,
            StartGame = 2,
            ExitGame = 3
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в игру 'Слова'." + '\n' + "Для продолжения нажмите любую клавишу:");
            Console.ReadKey();
            GameMechanics.SetDefaultSettings();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Нажмите 1 для изменения настроек." + '\n' + "Нажмите 2 для начала игры." + '\n' + "Нажмите 3 для выхода из игры.");
                try
                {
                    int key = int.Parse(Console.ReadLine());
                    switch ((MainMenuActions)key)
                    {
                        case MainMenuActions.SetCustomSettings:
                            GameMechanics.SetCustomSettings();
                            break;
                        case MainMenuActions.StartGame:
                            GameMechanics.BaseWordInput();
                            GameMechanics.StartGame();
                            break;
                        case MainMenuActions.ExitGame:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine("Такой опции нету в меню. Нажмите любую клавишу для продолжения и повторите ввод.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.Beep();
                    Console.WriteLine("Ошибка: 'Вводимое значение должно быть целым числом.'" + '\n' + "Нажмите любую клавишу для продолжения и повторите ввод.");
                    Console.ReadKey();
                }
            }
        }
    }
}
