using System;
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

namespace A
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        private static void Maximize() => ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 3);

        static void Main()
        {
            Maximize();
            string input;
            int steps = 0;
            Farm.AddCow(new Cow("Tedi", Sex.Female, 2, 2));

            //Farm.DrawField();
            while (true)
            {
                while (steps > 0)
                {
                    foreach (Cow cow in Farm.GetCows()) cow.Think();
                    steps--;
                    if (steps > 0)
                    {
                        Farm.DrawField();
                        Console.WriteLine(steps);
                        Thread.Sleep(250);
                    }
                }

                Farm.DrawField();
                input = Console.ReadLine();
                if (input == "") input = "1";

                if (int.TryParse(input, out steps)) continue;

                string[] comand = input.Split(' ');
                comand[0] = char.ToUpper(comand[0][0]) + comand[0].Substring(1);
                if (Farm.TryGetCow(comand[0], out Cow tryCow) && tryCow.TryMove(comand[1]) != MoveError.Invalid) continue;
            }

            //Console.ReadLine();
        }
    }

    public interface IReadOnlyWrapper<T>
    {
        T Value { get; }
    }

    public class Wrapper<T> : IReadOnlyWrapper<T> where T : struct
    {
        public T Value { get; set; }
    }
}
