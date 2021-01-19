using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Words.Mechanics
{
    static class GameMechanics
    {
        const double interval = 1000;
        const int defaultMinLength = 8;
        const int defaultMaxLength = 30;
        const int defaultRoundDuration = 10;
        static bool isAlive;
        static int minLength;
        static int maxLength;
        static int roundDuration;
        static int timeLeft;
        static Timer timer;
        static string baseWord;
        static Dictionary<char, int> baseWordDictionary;
        static string inputedWord;
        static List<string> countedWords = new List<string>();
        
        enum SettingsMenuActions
        {
            ChangeMinLength = 1,
            ChangeMaxLength = 2,
            ChangeRoundDuration = 3,
            ExitSettingsMenu = 4
        }

        public static void SetDefaultSettings()
        {
            minLength = defaultMinLength;
            maxLength = defaultMaxLength;
            timer = new Timer(interval);
            timer.Elapsed += TimerTick;
            timer.AutoReset = true;
            roundDuration = timeLeft = defaultRoundDuration;
        }

        public static bool Parse(string inputString, out int outputInt)
        {
            int tryParse;
            if (int.TryParse(inputString, out tryParse))
            {
                outputInt = tryParse;
                return true;
            }
            else
            {
                Console.Clear();
                Console.Beep();
                Console.WriteLine("Ошибка: 'Вводимое значение должно быть целым числом.'" + '\n' +
                                        "Нажмите любую клавишу для продолжения и повторите ввод.");
                Console.ReadKey();
                outputInt = 0;
                return false;
            }
        }

        public static void SetCustomSettings()
        {
            int Action;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Нажмите 1 для изменения минимальной длины слова." + '\n' + "Нажмите 2 для изменения максимальной длины слова." + '\n' + "Нажмите 3 для изменения длительности раунда." + '\n' + "Нажмите 4 для возврата в главное меню.");
                if (!Parse(Console.ReadLine(), out Action))
                    continue;
                else
                {
                    switch ((SettingsMenuActions)Action)
                    {
                        case SettingsMenuActions.ChangeMinLength:
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Введите минимальную длину слова:");
                                if (!Parse(Console.ReadLine(), out minLength))
                                    continue;
                                else
                                    break;
                            }
                            break;
                        case SettingsMenuActions.ChangeMaxLength:
                            while (true)
                            {

                                Console.Clear();
                                Console.WriteLine("Введите максимальную длину слова:");
                                if (!Parse(Console.ReadLine(), out maxLength))
                                    continue;
                                else
                                    break;
                            }
                            break;
                        case SettingsMenuActions.ChangeRoundDuration:
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Задайте длительность раунда в секундах:");
                                if (!Parse(Console.ReadLine(), out maxLength))
                                    continue;
                                else
                                {
                                    timeLeft = roundDuration;
                                    break;
                                }
                            }
                            break;
                        case SettingsMenuActions.ExitSettingsMenu:
                            exit = !exit;
                            Console.Clear();
                            Console.WriteLine("Параметры сохранены." + '\n' + "Нажмите любую клавишу для продолжения.");
                            Console.ReadKey();
                            break;
                        default:
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine("Такого пункта не существует в меню. Нажмите любую клавишу для продолжения и повторите ввод.");
                            Console.ReadKey();
                            continue;
                    }
                }
            }
        }
        public static void BaseWordInput()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Введите первоначальное слово:" + '\n' +
                    "Минимальная длина слова должна составлять: " + minLength.ToString() + '\n' +
                    "Максимальная длина слова должна составлять: " + maxLength.ToString());
                baseWord = Console.ReadLine();
                if (baseWord.Length > 0)
                    if (minLength <= baseWord.Length && baseWord.Length <= maxLength)
                    {
                        if (CheckBaseWord())
                        {
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine("Ошибка: Базовое слово не может содержать цифры и прочие знаки, кроме букв." + '\n' +
                                "Нажмите любую клавишу для продолжения и повторите ввод.");
                            Console.ReadKey();
                            continue;
                        }
                        int lettersCount = baseWord.ToCharArray().Distinct().Count();
                        baseWordDictionary = new Dictionary<char, int>(lettersCount);
                        foreach (var c in baseWord.ToUpper())
                        {
                            if (baseWordDictionary.Any(item => item.Key == c) != true)
                                baseWordDictionary.Add(c, baseWord.ToUpper().Count(letter => letter == c));
                        }
                        Console.Clear();
                        Console.WriteLine("Слово было сохранено." + '\n' +
                            "Нажмите любую клавишу для продолжения.");
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.Beep();
                        Console.WriteLine("Слово не подходит по правилам." + '\n' +
                            "Минимальная длина слова должна составлять: " + minLength.ToString() + '\n' +
                            "Максимальная длина слова должна составлять: " + maxLength.ToString() + '\n' +
                            "Нажмите любую клавишу для продолжения и повторите ввод.");
                        Console.ReadKey();
                        continue;
                    }
                else
                {
                    Console.Clear();
                    Console.Beep();
                    Console.WriteLine("Ошибка: Вы ввели пустую строку." + '\n' +
                        "Минимальная длина слова должна составлять: " + minLength.ToString() + '\n' +
                        "Максимальная длина слова должна составлять: " + maxLength.ToString() + '\n' +
                        "Нажмите любую клавишу для продолжения и повторите ввод.");
                    Console.ReadKey();
                    continue;
                }
            }
        }

        static void Round()
        {
            Console.Clear();
            timer.Start();
            Console.WriteLine('\n' + "Основное слово: {0}" + '\n' +
                "Введите полученное слово:", baseWord);
            inputedWord = Console.ReadLine();
            int kol = 0;
            if (inputedWord.Length > 0)
            {
                foreach (var c in inputedWord.ToUpper().Distinct())
                {
                    if (baseWordDictionary.ContainsKey(c))
                        if (inputedWord.ToUpper().Count(letter => letter == c) <= baseWordDictionary[c])
                            kol++;
                }
                if (kol == inputedWord.Distinct().Count())
                {
                    if (!countedWords.Any(item => item == inputedWord))
                    {
                        countedWords.Add(inputedWord);
                        Console.WriteLine("Слово засчитано.");
                        timer.Stop();
                        timeLeft = roundDuration;
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Такое слово уже было.");
                        Console.ReadKey();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Слово не засчитано.");
                    Console.ReadKey();
                    return;
                }
            }
        }

        public static void StartGame()
        {
            isAlive = true;
            BaseWordInput();
            while (isAlive)
                Round();
        }

        private static void TimerTick(Object source, ElapsedEventArgs e)
        {
            Console.Beep();
            int currentLineCursorTop = Console.CursorTop;
            int currentLineCursorLeft = Console.CursorLeft;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.Write("Осталось времени: {0} секунд(-ы)", timeLeft);
            Console.SetCursorPosition(currentLineCursorLeft, currentLineCursorTop);
            Console.CursorVisible = true;
            timeLeft -= 1;
            if (timeLeft == -1)
                StopRound();
        }

        private static void StopRound()
        {
            isAlive = false;
            timer.Stop();
            timeLeft = roundDuration;
            Console.Clear();
            Console.WriteLine("Вы смогли составить: {0} слов(-а) из {1}", countedWords.Count, baseWord);
            foreach (var word in countedWords)
                Console.WriteLine(word);
            Console.WriteLine("Нажмите любую клавишу для перехода в меню.");
        }
        private static bool CheckBaseWord()
        {
            if (Regex.Match(baseWord, @"^[а-яА-Я]+$").Success || Regex.Match(baseWord, @"^[A-Za-z]+$").Success)
                return false;
            else
                return true;
        }
    }
}
