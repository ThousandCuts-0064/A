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
        private readonly Wrapper<ColorChar> eyes0 = new Wrapper<ColorChar>() { Value = new ColorChar(ConsoleColor.Yellow, '"') };
        private readonly Wrapper<ColorChar> eyes1 = new Wrapper<ColorChar>() { Value = new ColorChar(ConsoleColor.Yellow, '=') };
        private readonly Wrapper<ColorChar> mouth0 = new Wrapper<ColorChar>() { Value = new ColorChar(ConsoleColor.Yellow, 'v') };
        private readonly Wrapper<ColorChar> mouth1 = new Wrapper<ColorChar>() { Value = new ColorChar(ConsoleColor.Yellow, '<') };

        private readonly Wrapper<ColorChar> frontLeg0 = new Wrapper<ColorChar>() { Value = new ColorChar(ConsoleColor.DarkYellow, '│') };
        private readonly Wrapper<ColorChar> backLeg0 = new Wrapper<ColorChar>() { Value = new ColorChar(ConsoleColor.DarkYellow, '│') };
        private readonly Wrapper<ColorChar> frontLeg1 = new Wrapper<ColorChar>() { Value = new ColorChar(ConsoleColor.DarkYellow, '─') };
        private readonly Wrapper<ColorChar> backLeg1 = new Wrapper<ColorChar>() { Value = new ColorChar(ConsoleColor.DarkYellow, '─') };
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

        private readonly Dictionary<bool, IReadOnlyWrapper<ColorChar>> eyes;
        private readonly Dictionary<bool, IReadOnlyWrapper<ColorChar>> mouth;
        private readonly Dictionary<bool, IReadOnlyWrapper<ColorChar>> frontLegs;
        private readonly Dictionary<bool, IReadOnlyWrapper<ColorChar>> backLegs;
        public override IReadOnlyDictionary<bool, IReadOnlyWrapper<ColorChar>> Eyes => eyes;
        public override IReadOnlyDictionary<bool, IReadOnlyWrapper<ColorChar>> Mouth => mouth;
        public override IReadOnlyDictionary<bool, IReadOnlyWrapper<ColorChar>> FrontLeg => frontLegs;
        public override IReadOnlyDictionary<bool, IReadOnlyWrapper<ColorChar>> BackLeg => backLegs;
        public float Milk { get; }

        public Cow(string name, Sex sex, int x, int y, float milk = 0) : base(name, sex, x, y)
        {
            Milk = milk;

            eyes = new Dictionary<bool, IReadOnlyWrapper<ColorChar>>()
            {
                { false, eyes0 },
                { true, eyes1 }
            };
            mouth = new Dictionary<bool, IReadOnlyWrapper<ColorChar>>()
            {
                { false, mouth0 },
                { true, mouth1 }
            };
            frontLegs = new Dictionary<bool, IReadOnlyWrapper<ColorChar>>()
            {
                { false, frontLeg0 },
                { true, frontLeg1 }
            };
            backLegs = new Dictionary<bool, IReadOnlyWrapper<ColorChar>>()
            {
                { false, backLeg0 },
                { true, backLeg1 }
            };

            Square = new Square(this);
            Square.OnMoveEvent += Animate;
        }

        public override void Animate()
        {
            if (legIndex == 0 || legIndex == frontLegs0.Length - 1) legReadDir *= -1;
            legIndex += legReadDir;
            frontLeg0.Value = frontLegs0[legIndex];
            backLeg0.Value = backLegs0[legIndex];
            frontLeg1.Value = frontLegs1[legIndex];
            backLeg1.Value = backLegs1[legIndex];
        }

        public override void Think()
        {
            Direction curr = Last;
            int rnd = new Random().Next(101);
            int dirRnd;
            if (rnd < 5) dirRnd = 0;
            else if (rnd < 15) dirRnd = 1;
            else if (rnd < 25) dirRnd = 2;
            else dirRnd = 3;

            switch (Last)
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
