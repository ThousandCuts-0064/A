using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    class Cow : Animal
    {
        private int legIndex = 1;
        private int legReadDir = 1;
        private readonly Dictionary<Direction, ColorChar> eyes = new Dictionary<Direction, ColorChar>()
        {
            { Direction.Up, new ColorChar(ConsoleColor.Yellow, '"') },
            { Direction.Down, new ColorChar(ConsoleColor.Yellow, '"') },
            { Direction.Right, new ColorChar(ConsoleColor.Yellow, '=') },
            { Direction.Left, new ColorChar(ConsoleColor.Yellow, '=') },
        };
        private readonly Dictionary<Direction, ColorChar> mouth = new Dictionary<Direction, ColorChar>()
        {
            { Direction.Up, new ColorChar(ConsoleColor.Yellow, 'v') },
            { Direction.Down, new ColorChar(ConsoleColor.Yellow, '^') },
            { Direction.Right, new ColorChar(ConsoleColor.Yellow, '<') },
            { Direction.Left, new ColorChar(ConsoleColor.Yellow, '>') },
        };
        private readonly Dictionary<Direction, ColorChar[]> frontLegs = new Dictionary<Direction, ColorChar[]>()
        {
            { Direction.Up, new ColorChar[]{
                new ColorChar(ConsoleColor.DarkYellow, '/'),
                new ColorChar(ConsoleColor.DarkYellow, '│'),
                new ColorChar(ConsoleColor.DarkYellow, '\\') } },
            { Direction.Down, new ColorChar[]{
                new ColorChar(ConsoleColor.DarkYellow, '\\'),
                new ColorChar(ConsoleColor.DarkYellow, '│'),
                new ColorChar(ConsoleColor.DarkYellow, '/') } },
            { Direction.Right, new ColorChar[]{
                new ColorChar(ConsoleColor.DarkYellow, '\\'),
                new ColorChar(ConsoleColor.DarkYellow, '─'),
                new ColorChar(ConsoleColor.DarkYellow, '/') } },
            { Direction.Left, new ColorChar[]{
                new ColorChar(ConsoleColor.DarkYellow, '/'),
                new ColorChar(ConsoleColor.DarkYellow, '─'),
                new ColorChar(ConsoleColor.DarkYellow, '\\') } },
        };
        private readonly Dictionary<Direction, ColorChar[]> backLegs = new Dictionary<Direction, ColorChar[]>()
        {
            { Direction.Up, new ColorChar[] {
                new ColorChar(ConsoleColor.DarkYellow, '\\'),
                new ColorChar(ConsoleColor.DarkYellow, '│'),
                new ColorChar(ConsoleColor.DarkYellow, '/') } },
            { Direction.Down, new ColorChar[] {
                new ColorChar(ConsoleColor.DarkYellow, '/'),
                new ColorChar(ConsoleColor.DarkYellow, '│'),
                new ColorChar(ConsoleColor.DarkYellow, '\\') } },
            { Direction.Right, new ColorChar[] {
                new ColorChar(ConsoleColor.DarkYellow, '/'),
                new ColorChar(ConsoleColor.DarkYellow, '─'),
                new ColorChar(ConsoleColor.DarkYellow, '\\') } },
            { Direction.Left, new ColorChar[] {
                new ColorChar(ConsoleColor.DarkYellow, '\\'),
                new ColorChar(ConsoleColor.DarkYellow, '─'),
                new ColorChar(ConsoleColor.DarkYellow, '/') } },
        };
        public float Milk { get; }

        public Cow(string name, Sex sex, int x, int y) : base(name, sex, x, y)
        {
            Milk = 0;

            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (dir == Direction.None) continue;
                Eyes[dir].Value = eyes[dir];
                Mouth[dir].Value = mouth[dir];
                FrontLeg[dir].Value = frontLegs[dir][1];
                BackLeg[dir].Value = backLegs[dir][1];
            }
        }

        public Cow(string name, Sex sex, int x, int y, float milk) : this(name, sex, x, y)
        {
            Milk = milk;
        }

        public override void Animate()
        {
            if (legIndex == 0 || legIndex == frontLegs[Direction.Up].Length - 1) legReadDir *= -1;
            legIndex += legReadDir;
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (dir == Direction.None) continue;
                FrontLeg[dir].Value = frontLegs[dir][legIndex];
                BackLeg[dir].Value = backLegs[dir][legIndex];
            }
        }

        public override void Think()
        {
            Direction curr = HitBox.LastMove;
            int rnd = new Random().Next(101);
            int dirRnd;
            if (rnd < 5) dirRnd = 0;
            else if (rnd < 15) dirRnd = 1;
            else if (rnd < 25) dirRnd = 2;
            else dirRnd = 3;

            switch (HitBox.LastMove)
            {
                case Direction.None:
                    if (rnd < 25) curr = Direction.Up;
                    else if (rnd < 50) curr = Direction.Down;
                    else if (rnd < 75) curr = Direction.Right;
                    else curr = Direction.Left;
                    break;

                case Direction.Up:
                    switch (dirRnd)
                    {
                        case 0: curr = Direction.None; break;
                        case 1: curr = Direction.Right; break;
                        case 2: curr = Direction.Left; break;
                    }
                    break;

                case Direction.Down:
                    switch (dirRnd)
                    {
                        case 0: curr = Direction.None; break;
                        case 1: curr = Direction.Right; break;
                        case 2: curr = Direction.Left; break;
                    }
                    break;

                case Direction.Right:
                    switch (dirRnd)
                    {
                        case 0: curr = Direction.None; break;
                        case 1: curr = Direction.Up; break;
                        case 2: curr = Direction.Down; break;
                    }
                    break;

                case Direction.Left:
                    switch (dirRnd)
                    {
                        case 0: curr = Direction.None; break;
                        case 1: curr = Direction.Up; break;
                        case 2: curr = Direction.Down; break;
                    }
                    break;
            }

            HitBox.TryMove(curr);
        }
    }
}
