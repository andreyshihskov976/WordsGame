﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Words.Mechanics;

namespace Words
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в игру 'Слова'." + '\n' + "Для продолжения нажмите любую клавишу:");
            Console.ReadKey();
            GameMechanics.DefaultSettings();
        menu:
            Console.Clear();
            Console.WriteLine("Нажмите 1 для изменения настроек." + '\n' + "Нажмите 2 для начала игры." + '\n' + "Нажмите 3 для выхода из игры.");
            try
            {
                int key = int.Parse(Console.ReadLine());
                switch (key)
                {
                    case 1:
                        GameMechanics.Settings();
                        break;
                    case 2:
                        GameMechanics.BaseWordInput();
                        GameMechanics.MainGame();
                        goto menu;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.Beep();
                        Console.WriteLine("Такой опции нету в меню. Нажмите любую клавишу для продолжения и повторите ввод.");
                        Console.ReadKey();
                        goto menu;
                }
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.Beep();
                Console.WriteLine("Ошибка: 'Вводимое значение должно быть целым числом.'" + '\n' + "Нажмите любую клавишу для продолжения и повторите ввод.");
                Console.ReadKey();
                goto menu;
            }
        }
    }
}
