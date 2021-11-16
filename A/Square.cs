using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    public struct ColorChar
    {
        public ConsoleColor ConsoleColor { get; }
        public char Char { get; }

        public ColorChar(ConsoleColor consoleColor, char @char)
        {
            ConsoleColor = consoleColor;
            Char = @char;
        }

        public ColorChar WithColor(ConsoleColor consoleColor) => new ColorChar(consoleColor, Char);
        public ColorChar WithChar(char @char) => new ColorChar(ConsoleColor, @char);
        public void Draw()
        {
            Console.ForegroundColor = ConsoleColor;
            Console.Write(Char);
        }
    }

    interface IReadOnlySquare
    {
        ColorChar ColorChar { get; }
        FarmObject FarmObject { get; }
    }

    class Square : IReadOnlySquare
    {
        public ColorChar ColorChar { get; set; }
        public FarmObject FarmObject { get; }

        public Square(Animal animal, ColorChar colorChar)
        {
            FarmObject = animal;
            ColorChar = colorChar;
        }
    }
}
