﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Threading;
using System.Reflection;

namespace A
{
    enum InputMode
    {
        Standard,
        Arcade,
    }

    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        public static Animal Select { get; set; } = null;
        public static InputMode InputMode { get; set; }

        static Program() => ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 3);

        static void Main()
        {
            string input;
            int steps = 0;
            Farm.TryAddAnimal(new Cow("Tedi", Sex.Female, 2, 2));

            //Farm.DrawField();
            while (true)
            {
                while (steps > 0)
                {
                    foreach (Animal animal in Farm.GetAnimals()) animal.Think();
                    steps--;
                    if (steps > 0)
                    {
                        Farm.DrawField();
                        Console.WriteLine(steps);
                        Thread.Sleep(100);
                    }
                }
                Farm.DrawField();

                switch (InputMode)
                {
                    case InputMode.Standard:
                        input = Console.ReadLine();
                        if (input == "") input = "1";

                        if (int.TryParse(input, out steps)) continue;

                        string[] comand = input.Split(' ').Where(str => str != "").Select(str => str = char.ToUpper(str[0]) + str.Substring(1).ToLower()).ToArray();
                        if (comand[0] == "New")
                        {
                            if (comand.Length < 6) continue;
                            if (!int.TryParse(comand[4], out int x)) continue;
                            if (!int.TryParse(comand[5], out int y)) continue;
                            if (!Enum.TryParse(comand[3], out Sex sex)) continue;
                            Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == comand[1]);
                            if (type == null || !typeof(Animal).IsAssignableFrom(type)) continue;

                            Farm.TryAddAnimal((Animal)Activator.CreateInstance(type, comand[2], sex, x, y));
                        }
                        else if (comand[0] == "Select")
                        {
                            if (Farm.TryGetAnimal(comand[1], out Animal select))
                            {
                                Select = select;
                                InputMode = InputMode.Arcade;
                            }
                            else Select = null;
                        }
                        else if (Select != null)
                        {
                            if (comand[0] == "Mode" && Enum.TryParse(comand[1], out InputMode inputMode)) InputMode = inputMode;
                            else if (comand[0] == "Deselect") Select = null;
                            else if (comand[0] == "Flip") Select.AnimalBody.Flip();
                            else if (Select.Body.TryMove(comand[0]) != MoveError.Invalid) continue;
                        }
                        else if (Farm.TryGetAnimal(comand[0], out Animal tryAnimal))
                        {
                            if (comand[1] == "Flip") tryAnimal.AnimalBody.Flip();
                            else if (tryAnimal.Body.TryMove(comand[1]) != MoveError.Invalid) continue;
                        }
                        break;

                    case InputMode.Arcade:
                        Console.CursorVisible = false;
                        ConsoleKeyInfo key = Console.ReadKey();

                        switch (key.Key)
                        {
                            case ConsoleKey.W:
                                Select.Body.TryMove(Direction.Up);
                                break;

                            case ConsoleKey.A:
                                Select.Body.TryMove(Direction.Left);
                                break;

                            case ConsoleKey.S:
                                Select.Body.TryMove(Direction.Down);
                                break;

                            case ConsoleKey.D:
                                Select.Body.TryMove(Direction.Right);
                                break;

                            case ConsoleKey.F:
                                Select.AnimalBody.Flip();
                                break;

                            case ConsoleKey.Escape:
                                InputMode = InputMode.Standard;
                                Select = null;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        InputMode = InputMode.Standard;
                        break;
                }
            }

            //Console.ReadLine();
        }
    }
}
