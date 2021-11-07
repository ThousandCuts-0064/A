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
        public int Size { get; }
        public int Fat { get; }
        public Animal Animal { get; }
        public Square(Animal animal, int size)
        {
            Animal = animal;
            Size = size;
            Fat = (size - 1) / 2;
        }

        public void Move(Direction dir)
        {
            OnMoveEvent.Invoke();
            switch (dir)
            {
                case Direction.Up:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[Animal.X - 1 + j - 1, Animal.Y - 1 + i] = Animal.ReadBody[j, i].Value;
                        Farm.Field[Animal.X + 1, Animal.Y - 1 + i] = Farm.Empty;
                    }
                    break;

                case Direction.Down:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[Animal.X + 1 - j + 1, Animal.Y - 1 + i] = Animal.ReadBody[Size - 1 - j, i].Value;
                        Farm.Field[Animal.X - 1, Animal.Y - 1 + i] = Farm.Empty;
                    }
                    break;

                case Direction.Right:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[Animal.X - 1 + i, Animal.Y + 1 - j + 1] = Animal.ReadBody[i, Size - 1 - j].Value;
                        Farm.Field[Animal.X - 1 + i, Animal.Y - 1] = Farm.Empty;
                    }
                    break;

                case Direction.Left:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[Animal.X - 1 + i, Animal.Y - 1 + j - 1] = Animal.ReadBody[i, j].Value;
                        Farm.Field[Animal.X - 1 + i, Animal.Y + 1] = Farm.Empty;
                    }
                    break;
            }
        }
    }
}
