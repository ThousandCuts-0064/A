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
            Console.ResetColor();
        }
    }

    class Square
    {
        public delegate void OnMove();
        public event OnMove OnMoveEvent;
        public const int size = 3;
        public Animal Animal { get; }
        public IReadOnlyWrapper<ColorChar>[,] Space { get; private set; }
        public Square(Animal animal)
        {
            Animal = animal;
            Space = new IReadOnlyWrapper<ColorChar>[size, size]
            {
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Animal.Eyes[false] },
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Red, 'O') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Animal.Mouth[false] },
                { Animal.BackLeg[false], Animal.SexColorChar, Animal.FrontLeg[false] }
            };
        }

        public void Move(Direction dir)
        {
            OnMoveEvent.Invoke();
            switch (dir)
            {
                case Direction.Up:
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                            Farm.Field[Animal.X - 1 + j - 1, Animal.Y - 1 + i] = Space[j, i].Value;
                        Farm.Field[Animal.X + 1, Animal.Y - 1 + i] = Farm.Empty;
                    }
                    break;

                case Direction.Down:
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                            Farm.Field[Animal.X + 1 - j + 1, Animal.Y - 1 + i] = Space[size - 1 - j, i].Value;
                        Farm.Field[Animal.X - 1, Animal.Y - 1 + i] = Farm.Empty;
                    }
                    break;

                case Direction.Right:
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                            Farm.Field[Animal.X - 1 + i, Animal.Y + 1 - j + 1] = Space[i, size - 1 - j].Value;
                        Farm.Field[Animal.X - 1 + i, Animal.Y - 1] = Farm.Empty;
                    }
                    break;

                case Direction.Left:
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                            Farm.Field[Animal.X - 1 + i, Animal.Y - 1 + j - 1] = Space[i, j].Value;
                        Farm.Field[Animal.X - 1 + i, Animal.Y + 1] = Farm.Empty;
                    }
                    break;
            }
        }
    }
}
