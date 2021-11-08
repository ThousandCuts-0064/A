using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    public enum Sex
    {
        None,
        Male,
        Female,
        Both,
        Undefined,
        Unknown,
    }

    public static class SexExt
    {
        public static ColorChar ToColorChar(this Sex sex)
        {
            switch (sex)
            {
                case Sex.None: return new ColorChar(ConsoleColor.White, '*');
                case Sex.Male: return new ColorChar(ConsoleColor.Blue, '1');
                case Sex.Female: return new ColorChar(ConsoleColor.Red, '0');
                case Sex.Both: return new ColorChar(ConsoleColor.Magenta, '&');
                case Sex.Undefined: return new ColorChar(ConsoleColor.Cyan, '%');
                case Sex.Unknown: return new ColorChar(ConsoleColor.Gray, '?');
                default: return new ColorChar(ConsoleColor.Black, '@');
            }
        }
    }

    public enum Direction
    {
        None,
        Up,
        Down,
        Right,
        Left,
    }

    public enum MoveError
    {
        None,
        Stamina,
        Blocked,
        Invalid,
    }

    abstract class Animal
    {
        //¦ °
        private Wrapper<ColorChar> sexColorChar = new Wrapper<ColorChar>();
        private Sex sex;
        protected Directional2DArray<IReadOnlyWrapper<ColorChar>> Body { get; }
        protected Dictionary<Direction, Wrapper<ColorChar>> Eyes { get; } = new Dictionary<Direction, Wrapper<ColorChar>>()
        {
            { Direction.Up, new Wrapper<ColorChar>() },
            { Direction.Down, new Wrapper<ColorChar>() },
            { Direction.Right, new Wrapper<ColorChar>() },
            { Direction.Left, new Wrapper<ColorChar>() },
        };
        protected Dictionary<Direction, Wrapper<ColorChar>> Mouth { get; } = new Dictionary<Direction, Wrapper<ColorChar>>()
        {
            { Direction.Up, new Wrapper<ColorChar>() },
            { Direction.Down, new Wrapper<ColorChar>() },
            { Direction.Right, new Wrapper<ColorChar>() },
            { Direction.Left, new Wrapper<ColorChar>() },
        };
        protected Dictionary<Direction, Wrapper<ColorChar>> FrontLeg = new Dictionary<Direction, Wrapper<ColorChar>>()
        {
            { Direction.Up, new Wrapper<ColorChar>() },
            { Direction.Down, new Wrapper<ColorChar>() },
            { Direction.Right, new Wrapper<ColorChar>() },
            { Direction.Left, new Wrapper<ColorChar>() },
        };
        protected Dictionary<Direction, Wrapper<ColorChar>> BackLeg = new Dictionary<Direction, Wrapper<ColorChar>>()
        {
            { Direction.Up, new Wrapper<ColorChar>() },
            { Direction.Down, new Wrapper<ColorChar>() },
            { Direction.Right, new Wrapper<ColorChar>() },
            { Direction.Left, new Wrapper<ColorChar>() },
        };
        public IReadOnlyDirectional2DArray<IReadOnlyWrapper<ColorChar>> ReadBody => Body;
        public Square Square { get; }
        public Direction LastMove { get; protected set; }
        public Sex Sex
        {
            get => sex;
            protected set
            {
                sex = value;
                sexColorChar.Value = value.ToColorChar();
            }
        }
        public IReadOnlyWrapper<ColorChar> SexColorChar => sexColorChar;
        public string Name { get; }
        public int X { get; private set; }
        public int Y { get; private set; }


        public Animal(string name, Sex sex, int x, int y)
        {
            Name = name;
            Sex = sex;
            X = x;
            Y = y;

            Body = new Directional2DArray<IReadOnlyWrapper<ColorChar>>(new IReadOnlyWrapper<ColorChar>[,]
            {
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Eyes[Direction.Up] },
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Red, 'O') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Mouth[Direction.Up] },
                { BackLeg[Direction.Up], SexColorChar, FrontLeg[Direction.Up] }
            },
            new IReadOnlyWrapper<ColorChar>[,]
            {
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Eyes[Direction.Down] },
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Red, 'O') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Mouth[Direction.Down] },
                { BackLeg[Direction.Down], SexColorChar, FrontLeg[Direction.Down] }
            },
            new IReadOnlyWrapper<ColorChar>[,]
            {
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Eyes[Direction.Right] },
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Red, 'O') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Mouth[Direction.Right] },
                { BackLeg[Direction.Right], SexColorChar, FrontLeg[Direction.Right] }
            },
            new IReadOnlyWrapper<ColorChar>[,]
            {
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Eyes[Direction.Left] },
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Red, 'O') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Mouth[Direction.Left] },
                { BackLeg[Direction.Left], SexColorChar, FrontLeg[Direction.Left] }
            });

            Square = new Square(this, 3);
            Square.OnMoveEvent += Animate;
        }

        public abstract void Think();
        public abstract void Animate();

        public void Flip() => Body.Flipped = !Body.Flipped;

        public MoveError TryMove(Direction dir)
        {
            switch (dir)
            {
                case Direction.None: return MoveError.None;

                case Direction.Up:
                    if (X <= Square.Fat)
                    {
                        LastMove = Direction.None;
                        return MoveError.Blocked;
                    }

                    LastMove = Direction.Up;
                    Body.Direction = LastMove;
                    Square.Move(dir);
                    X -= 1;
                    return MoveError.None;

                case Direction.Down:
                    if (X >= Farm.N - 1 - Square.Fat)
                    {
                        LastMove = Direction.None;
                        return MoveError.Blocked;
                    }

                    LastMove = Direction.Down;
                    Body.Direction = LastMove;
                    Square.Move(dir);
                    X += 1;
                    return MoveError.None;


                case Direction.Right:
                    if (Y >= Farm.N - 1 - Square.Fat)
                    {
                        LastMove = Direction.None;
                        return MoveError.Blocked;
                    }

                    LastMove = Direction.Right;
                    Body.Direction = LastMove;
                    Square.Move(dir);
                    Y += 1;
                    return MoveError.None;


                case Direction.Left:
                    if (Y <= Square.Fat)
                    {
                        LastMove = Direction.None;
                        return MoveError.Blocked;
                    }

                    LastMove = Direction.Left;
                    Body.Direction = LastMove;
                    Square.Move(dir);
                    Y -= 1;
                    return MoveError.Blocked;

                default:
                    LastMove = Direction.None;
                    return MoveError.Invalid;
            }
        }

        public MoveError TryMove(string dir)
        {
            if (!Enum.TryParse(dir, out Direction direction)) return MoveError.Invalid;

            return TryMove(direction);
        }
    }
}
