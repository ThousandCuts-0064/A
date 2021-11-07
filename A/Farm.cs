using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    static class Farm
    {
        private static int n;
        private static readonly Dictionary<string, Cow> cows = new Dictionary<string, Cow>();
        public static ColorChar[,] Field { get; private set; }
        public static int N
        {
            get => n;
            set
            {
                n = value;
                Field = new ColorChar[value, value];
                EmptyField(value);
            }
        }
        public static ColorChar Empty { get; set; } = new ColorChar(ConsoleColor.DarkGreen, '#');

        static Farm() => N = 25;

        public static Cow GetCow(string name) => cows[name];
        public static bool TryGetCow(string name, out Cow cow) => cows.TryGetValue(name, out cow);
        public static Dictionary<string, Cow>.ValueCollection GetCows() => cows.Values;

        public static void AddCow(Cow cow)
        {
            //TODO: Check Size

            cows.Add(cow.Name, cow);
            for (int i = 0; i < cow.Square.Size; i++)
            {
                for (int y = 0; y < cow.Square.Size; y++)
                    Field[cow.X - 1 + i, cow.Y - 1 + y] = cow.ReadBody[i, y].Value;
            }
        }

        public static void DrawField()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine('┌' + new string('─', n) + '┐');
            for (int i = 0; i < n; i++)
            {
                Console.Write("│");
                for (int y = 0; y < n; y++)
                    Field[i, y].Draw();
                Console.Write('│');
                Console.WriteLine();
            }
            Console.WriteLine('└' + new string('─', n) + '┘');
            Console.Write(new string(' ', 100) + "\r");
        }

        private static void EmptyField(int n)
        {
            for (int i = 0; i < n; i++)
                for (int y = 0; y < n; y++)
                    Field[i, y] = Empty;
        }
    }
}
