using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    public enum AddAnimalError
    {
        None,
        NameTaken,
        NoSpace,
    }

    static class Farm
    {
        private static int size;
        private static readonly Dictionary<string, Animal> animals = new Dictionary<string, Animal>();

        public static IReadOnlySquare[,] Field { get; private set; }
        public static int Size
        {
            get => size;
            set
            {
                size = value;
                Field = new IReadOnlySquare[value, value];
                EmptyField(value);
            }
        }
        public static IReadOnlySquare Empty { get; set; } = new Square(null, new ColorChar(ConsoleColor.DarkGreen, '#'));

        static Farm() => Size = 25;

        public static Animal GetAnimal(string name) => animals[name];
        public static bool TryGetAnimal(string name, out Animal animal) => animals.TryGetValue(name, out animal);
        public static Dictionary<string, Animal>.ValueCollection GetAnimals() => animals.Values;

        public static AddAnimalError TryAddAnimal(Animal animal)
        {
            if (animals.ContainsKey(animal.Name)) return AddAnimalError.NameTaken;
            if (animal.HitBox.X - animal.HitBox.Fat < 0 ||
                animal.HitBox.Y - animal.HitBox.Fat < 0 ||
                animal.HitBox.X + animal.HitBox.Fat > size - 1 ||
                animal.HitBox.Y + animal.HitBox.Fat > size - 1)
                return AddAnimalError.NoSpace;

            animals.Add(animal.Name, animal);
            for (int i = 0; i < animal.HitBox.Size; i++)
                for (int y = 0; y < animal.HitBox.Size; y++)
                    Field[animal.HitBox.X - 1 + i, animal.HitBox.Y - 1 + y] = animal.ReadBody[i, y];
            return AddAnimalError.None;
        }

        public static void DrawField()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine('┌' + new string('─', size) + '┐');
            for (int i = 0; i < size; i++)
            {
                new ColorChar(ConsoleColor.White, '│').Draw();
                for (int y = 0; y < size; y++)
                    Field[i, y].ColorChar.Draw();
                new ColorChar(ConsoleColor.White, '│').Draw();
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine('└' + new string('─', size) + '┘');
            Console.WriteLine(new string(' ', 100) + "\rSelect: " + (Program.Select?.Name ?? "None"));
            Console.WriteLine(new string(' ', 100) + "\rMode:   " + Program.InputMode);
            Console.WriteLine();
            Console.Write(new string(' ', 100) + "\r");
            Console.CursorVisible = true;
        }

        private static void EmptyField(int n)
        {
            for (int i = 0; i < n; i++)
                for (int y = 0; y < n; y++)
                    Field[i, y] = Empty;
        }
    }
}
