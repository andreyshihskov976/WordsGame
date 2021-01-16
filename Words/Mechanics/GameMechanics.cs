using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Words.Mechanics
{
    static class GameMechanics
    {
        static int minLength, maxLength, roundDuration, timeLeft;
        static Timer timer;
        static Thread mainGameThread;
        static string baseWord;
        static Dictionary<char, int> baseWordDictionary;
        static List<string> countedWords = new List<string>();
        static List<char> numbers = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        static string inputedWord;
        

        public static void DefaultSettings()
        {
            minLength = 8;
            maxLength = 30;
            timer = new Timer(1000);
            timer.Elapsed += Timer_Tick;
            timer.AutoReset = true;
            roundDuration = timeLeft = 5;
        }
        public static void Settings()
        {
            settingsMenu:
            Console.Clear();
            Console.WriteLine("Нажмите 1 для изменения минимальной длины слова." + '\n' + "Нажмите 2 для изменения максимальной длины слова." + '\n' + "Нажмите 3 для изменения длительности раунда.");
            try
            {
                int key = int.Parse(Console.ReadLine());
                switch (key)
                {
                    case 1:
                        goto minLengthInput;
                    case 2:
                        goto maxLengthInput;
                    case 3:
                        goto roundDurationInput;
                    default:
                        Console.Clear();
                        Console.Beep();
                        Console.WriteLine("Такого пункта не существует в меню. Нажмите любую клавишу для продолжения и повторите ввод.");
                        Console.ReadKey();
                        goto settingsMenu;
                }
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.Beep();
                Console.WriteLine("Ошибка: 'Вводимое значение должно быть целым числом.'" + '\n' + "Нажмите любую клавишу для продолжения и повторите ввод.");
                Console.ReadKey();
                goto settingsMenu;
            }
        minLengthInput:
            try
            {
                Console.Clear();
                Console.WriteLine("Введите минимальную длину слова:");
                minLength = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.Beep();
                Console.WriteLine("Ошибка: 'Минимальная длина слова должна указываться целым числом.'" + '\n' +
                    "Нажмите любую клавишу для продолжения и повторите ввод.");
                Console.ReadKey();
                goto minLengthInput;
            }
        maxLengthInput:
            try
            {
                Console.Clear();
                Console.WriteLine("Введите максимальную длину слова:");
                maxLength = int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.Beep();
                Console.WriteLine("Ошибка: 'Максимальная длина слова должна указываться целым числом.'" + '\n' +
                    "Нажмите любую клавишу для продолжения и повторите ввод.");
                Console.ReadKey();
                goto maxLengthInput;
            }
        roundDurationInput:
            try
            {
                Console.Clear();
                Console.WriteLine("Задайте длительность раунда в секундах:");
                roundDuration = int.Parse(Console.ReadLine());
                timeLeft = roundDuration;
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.Beep();
                Console.WriteLine("Ошибка: 'Длительность раунда должна указываться целым числом.'" + '\n' +
                    "Нажмите любую клавишу для продолжения и повторите ввод.");
                Console.ReadKey();
                goto roundDurationInput;
            }
            Console.WriteLine("Параметры сохранены.");
        }
        public static void BaseWordInput()
        {
        repeatInput:
            Console.Clear();
            Console.WriteLine("Введите первоначальное слово:" + '\n' +
                "Минимальная длина слова должна составлять: " + minLength.ToString() + '\n' +
                "Максимальная длина слова должна составлять: " + maxLength.ToString());
            baseWord = Console.ReadLine();
            if (baseWord.Length > 0)
                if (minLength <= baseWord.Length && baseWord.Length <= maxLength)
                {
                    foreach (char num in numbers)
                        if (baseWord.Any(letter => letter == num))
                        {
                            Console.Clear();
                            Console.Beep();
                            Console.WriteLine("Ошибка: Базовое слово не может содержать цифры и прочие знаки, кроме букв." + '\n' +
                                "Нажмите любую клавишу для продолжения и повторите ввод.");
                            Console.ReadKey();
                            goto repeatInput;
                        }
                    int lettersCount = baseWord.ToCharArray().Distinct().Count();
                    baseWordDictionary = new Dictionary<char, int>(lettersCount);
                    foreach (var c in baseWord.ToUpper())
                    {
                        if (baseWordDictionary.Any(item => item.Key == c) != true)
                            baseWordDictionary.Add(c, baseWord.ToUpper().Count(letter => letter == c));
                    }
                    Console.WriteLine("Слово было сохранено." + '\n' +
                        "Нажмите любую клавишу для продолжения.");
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
                    goto repeatInput;
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
                goto repeatInput;
            }
            Console.ReadKey();
        }

        static void Round()
        {
        link1:
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
                        goto link1;
                    }
                    else
                    {
                        Console.WriteLine("Такое слово уже было.");
                        Console.ReadKey();
                        goto link1;
                    }
                }
                else
                {
                    Console.WriteLine("Слово не засчитано.");
                    Console.ReadKey();
                    goto link1;
                }
            }
        }

        public static void MainGame()
        {
            mainGameThread = new Thread(new ThreadStart(Round));
            mainGameThread.Start();
            mainGameThread.Join();
        }

        private static void Timer_Tick(Object source, ElapsedEventArgs e)
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
                Stop_Round();
        }

        private static void Stop_Round()
        {
            timer.Stop();
            timeLeft = roundDuration;
            mainGameThread.Abort();
            mainGameThread = null;
            Console.Clear();
            Console.WriteLine("Вы смогли составить: {0} слов(-а) из {1}", countedWords.Count, baseWord);
            foreach (var word in countedWords)
                Console.WriteLine(word);
            Console.WriteLine("Нажмите любую клавишу для перехода в меню.");
        }
    }
}
