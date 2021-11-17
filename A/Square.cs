using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    public struct ColorChar
    {
        public ConsoleColor Color { get; }
        public char Char { get; }

        public ColorChar(ConsoleColor consoleColor, char @char)
        {
            Color = consoleColor;
            Char = @char;
        }

        public ColorChar WithColor(ConsoleColor consoleColor) => new ColorChar(consoleColor, Char);
        public ColorChar WithChar(char @char) => new ColorChar(Color, @char);
        public void Draw()
        {
            Console.ForegroundColor = Color;
            Console.Write(Char);
        }

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(ColorChar l, ColorChar r) => l.Color == r.Color && l.Char == r.Char;
        public static bool operator !=(ColorChar l, ColorChar r) => l.Color != r.Color || l.Char != r.Char;
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

        public Square(FarmObject owner, ColorChar colorChar)
        {
            FarmObject = owner;
            ColorChar = colorChar;
        }
    }
}
