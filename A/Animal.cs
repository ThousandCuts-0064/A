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
        private Direction face = Direction.Right;
        public bool Rotate { get; protected set; }
        public bool Flipped { get; protected set; }
        public abstract IReadOnlyDictionary<bool, IReadOnlyWrapper<ColorChar>> Eyes { get; }
        public abstract IReadOnlyDictionary<bool, IReadOnlyWrapper<ColorChar>> Mouth { get; }
        public abstract IReadOnlyDictionary<bool, IReadOnlyWrapper<ColorChar>> FrontLeg { get; }
        public abstract IReadOnlyDictionary<bool, IReadOnlyWrapper<ColorChar>> BackLeg { get; }
        public Square Square { get; protected set; }
        public IReadOnlyRotatable2DArray<IReadOnlyWrapper<ColorChar>> Body { get; protected set; }
        public Direction Last { get; protected set; }
        public Direction Face 
        {
            get => face; 
            protected set
            {
                if (face == value) return;


                face = value;
            }
        }
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
                        Last = Direction.None;
                        return MoveError.Blocked;
                    }

                    Square.Move(dir);
                    X -= 1;
                    Last = Direction.Up;
                    Face = Last;
                    return MoveError.None;

                case Direction.Down:
                    if (X >= Farm.N - 1)
                    {
                        Last = Direction.None;
                        return MoveError.Blocked;
                    }

                    Square.Move(dir);
                    X += 1;
                    Last = Direction.Down;
                    Face = Last;
                    return MoveError.None;


                case Direction.Right:
                    if (Y >= Farm.N - 1)
                    {
                        Last = Direction.None;
                        return MoveError.Blocked;
                    }

                    Square.Move(dir);
                    Y += 1;
                    Last = Direction.Right;
                    Face = Last;
                    return MoveError.None;


                case Direction.Left:
                    if (Y <= 0)
                    {
                        Last = Direction.None;
                        return MoveError.Blocked;
                    }

                    Square.Move(dir);
                    Y -= 1;
                    Last = Direction.Left;
                    Face = Last;
                    return MoveError.Blocked;

                default:
                    Last = Direction.None;
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
