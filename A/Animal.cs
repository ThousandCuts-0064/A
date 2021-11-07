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
        protected Dictionary<bool, Wrapper<ColorChar>> Eyes { get; } = new Dictionary<bool, Wrapper<ColorChar>>()
        {
            { false, new Wrapper<ColorChar>() },
            { true, new Wrapper<ColorChar>() }
        };
        protected Dictionary<bool, Wrapper<ColorChar>> Mouth { get; } = new Dictionary<bool, Wrapper<ColorChar>>()
        {
            { false, new Wrapper<ColorChar>() },
            { true, new Wrapper<ColorChar>() }
        };
        protected Dictionary<bool, Wrapper<ColorChar>> FrontLeg { get; } = new Dictionary<bool, Wrapper<ColorChar>>()
        {
            { false, new Wrapper<ColorChar>() },
            { true, new Wrapper<ColorChar>() }
        };
        protected Dictionary<bool, Wrapper<ColorChar>> BackLeg { get; } = new Dictionary<bool, Wrapper<ColorChar>>()
        {
            { false, new Wrapper<ColorChar>() },
            { true, new Wrapper<ColorChar>() }
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
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Eyes[false] },
                { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Red, 'O') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Mouth[false] },
                { BackLeg[false], SexColorChar, FrontLeg[false] }
            });

            Square = new Square(this);
            Square.OnMoveEvent += Animate;
        }

        public abstract void Think();
        public abstract void Animate();

        public MoveError TryMove(Direction dir)
        {
            switch (dir)
            {
                case Direction.None: return MoveError.None;

                case Direction.Up:
                    if (X <= 0)
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
                    if (X >= Farm.N - 1)
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
                    if (Y >= Farm.N - 1)
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
                    if (Y <= 0)
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
            dir = char.ToUpper(dir[0]) + dir.Substring(1);
            if (!Enum.TryParse(dir, out Direction direction)) return MoveError.Invalid;

            return TryMove(direction);
        }
    }
}
