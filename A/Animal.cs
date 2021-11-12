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
        private readonly Wrapper<ColorChar> sexColorChar = new Wrapper<ColorChar>();
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
        protected Dictionary<Direction, Wrapper<ColorChar>> FrontLeg { get; } = new Dictionary<Direction, Wrapper<ColorChar>>()
        {
            { Direction.Up, new Wrapper<ColorChar>() },
            { Direction.Down, new Wrapper<ColorChar>() },
            { Direction.Right, new Wrapper<ColorChar>() },
            { Direction.Left, new Wrapper<ColorChar>() },
        };
        protected Dictionary<Direction, Wrapper<ColorChar>> BackLeg { get; } = new Dictionary<Direction, Wrapper<ColorChar>>()
        {
            { Direction.Up, new Wrapper<ColorChar>() },
            { Direction.Down, new Wrapper<ColorChar>() },
            { Direction.Right, new Wrapper<ColorChar>() },
            { Direction.Left, new Wrapper<ColorChar>() },
        };
        public IReadOnlyDirectional2DArray<IReadOnlyWrapper<ColorChar>> ReadBody => Body;
        public HitBox HitBox { get; }
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
        

        public Animal(string name, Sex sex, int x, int y)
        {
            Name = name;
            Sex = sex;

            Dictionary<Direction, IReadOnlyWrapper<ColorChar>[,]> directions = new Dictionary<Direction, IReadOnlyWrapper<ColorChar>[,]>();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (dir == Direction.None) continue;
                directions[dir] = new IReadOnlyWrapper<ColorChar>[,]
                {
                    { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Eyes[dir] },
                    { new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Red, 'O') }, new Wrapper<ColorChar>() {Value = new ColorChar(ConsoleColor.Black, 'X') }, Mouth[dir] },
                    { BackLeg[dir], SexColorChar, FrontLeg[dir] }
                };
            }
            Body = new Directional2DArray<IReadOnlyWrapper<ColorChar>>(directions[Direction.Up], directions[Direction.Down], directions[Direction.Right], directions[Direction.Left]);

            HitBox = new HitBox(this, 3, x, y);
            HitBox.OnMoveEvent += Animate;
        }

        public abstract void Think();
        public abstract void Animate();

        public void Flip()
        {
            Body.Flipped = !Body.Flipped;
            HitBox.Update();
        }

        public void SetDirection(Direction dir) => Body.Direction = dir;
    }
}
