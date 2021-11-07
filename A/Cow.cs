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
        private readonly ColorChar eyes0 = new ColorChar(ConsoleColor.Yellow, '"');
        private readonly ColorChar eyes1 = new ColorChar(ConsoleColor.Yellow, '=');
        private readonly ColorChar mouth0 = new ColorChar(ConsoleColor.Yellow, 'v');
        private readonly ColorChar mouth1 = new ColorChar(ConsoleColor.Yellow, '<');
        private readonly ColorChar[] frontLegs0 = new ColorChar[] 
        {
            new ColorChar(ConsoleColor.DarkYellow, '/'),
            new ColorChar(ConsoleColor.DarkYellow, '│'),
            new ColorChar(ConsoleColor.DarkYellow, '\\')
        };
        private readonly ColorChar[] backLegs0 = new ColorChar[]
        {
            new ColorChar(ConsoleColor.DarkYellow, '\\'),
            new ColorChar(ConsoleColor.DarkYellow, '│'),
            new ColorChar(ConsoleColor.DarkYellow, '/')
        };
        private readonly ColorChar[] frontLegs1 = new ColorChar[]
        {
            new ColorChar(ConsoleColor.DarkYellow, '\\'),
            new ColorChar(ConsoleColor.DarkYellow, '─'),
            new ColorChar(ConsoleColor.DarkYellow, '/')
        };
        private readonly ColorChar[] backLegs1 = new ColorChar[]
        {
            new ColorChar(ConsoleColor.DarkYellow, '/'),
            new ColorChar(ConsoleColor.DarkYellow, '─'),
            new ColorChar(ConsoleColor.DarkYellow, '\\')
        };
        public float Milk { get; }

        public Cow(string name, Sex sex, int x, int y, float milk = 0) : base(name, sex, x, y)
        {
            Milk = milk;

            Eyes[false].Value = eyes0;
            Eyes[true].Value = eyes1;
            Mouth[false].Value = mouth0;
            Mouth[true].Value = mouth1;
            FrontLeg[false].Value = frontLegs0[1];
            FrontLeg[true].Value = frontLegs1[1];
            BackLeg[false].Value = backLegs0[1];
            BackLeg[true].Value = backLegs1[1];
        }

        public override void Animate()
        {
            if (legIndex == 0 || legIndex == frontLegs0.Length - 1) legReadDir *= -1;
            legIndex += legReadDir;
            FrontLeg[false].Value = frontLegs0[legIndex];
            FrontLeg[true].Value = frontLegs1[legIndex];
            BackLeg[false].Value = backLegs0[legIndex];
            BackLeg[true].Value = backLegs1[legIndex];
        }

        public override void Think()
        {
            Direction curr = LastMove;
            int rnd = new Random().Next(101);
            int dirRnd;
            if (rnd < 5) dirRnd = 0;
            else if (rnd < 15) dirRnd = 1;
            else if (rnd < 25) dirRnd = 2;
            else dirRnd = 3;

            switch (LastMove)
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

            TryMove(curr);
        }
    }
}
