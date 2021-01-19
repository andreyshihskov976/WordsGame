using System;
using static Words.Mechanics.GameMechanics;

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
            int Action;
            Console.WriteLine("Добро пожаловать в игру 'Слова'." + '\n' + "Для продолжения нажмите любую клавишу:");
            Console.ReadKey();
            SetDefaultSettings();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Нажмите 1 для изменения настроек." + '\n' + "Нажмите 2 для начала игры." + '\n' + "Нажмите 3 для выхода из игры.");
                if (Parse(Console.ReadLine(), out Action))
                    switch ((MainMenuActions)Action)
                    {
                        case MainMenuActions.SetCustomSettings:
                            SetCustomSettings();
                            break;
                        case MainMenuActions.StartGame:
                            StartGame();
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
        }
    }
}
