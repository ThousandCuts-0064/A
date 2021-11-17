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

    abstract class Animal : FarmObject
    {
        private readonly Square sexSquare;
        private Sex sex;
        protected Directional2DArray<IReadOnlySquare> Body { get; }
        protected Dictionary<Direction, Square> Eyes { get; }
        protected Dictionary<Direction, Square> Mouth { get; }
        protected Dictionary<Direction, Square> FrontLeg { get; }
        protected Dictionary<Direction, Square> BackLeg { get; }
        protected AnimalHitBox AnimalHitBox { get; }
        public override HitBox HitBox => AnimalHitBox;
        public IReadOnlyDirectional2DArray<IReadOnlySquare> ReadBody => Body;
        public Sex Sex
        {
            get => sex;
            protected set
            {
                sex = value;
                sexSquare.ColorChar = value.ToColorChar();
            }
        }
        public IReadOnlySquare SexColorChar => sexSquare;
        public string Name { get; }


        public Animal(string name, Sex sex, int x, int y) : base(x, y)
        {
            Name = name;
            sexSquare = new Square(this, sex.ToColorChar());
            Sex = sex;

            Eyes = new Dictionary<Direction, Square>()
            {
                { Direction.Up, new Square(this, default) },
                { Direction.Down, new Square(this, default) },
                { Direction.Right, new Square(this, default) },
                { Direction.Left, new Square(this, default) },
            };
            Mouth = new Dictionary<Direction, Square>()
            {
                { Direction.Up, new Square(this, default) },
                { Direction.Down, new Square(this, default) },
                { Direction.Right, new Square(this, default) },
                { Direction.Left, new Square(this, default) },
            };
            FrontLeg = new Dictionary<Direction, Square>()
            {
                { Direction.Up, new Square(this, default) },
                { Direction.Down, new Square(this, default) },
                { Direction.Right, new Square(this, default) },
                { Direction.Left, new Square(this, default) },
            };
            BackLeg = new Dictionary<Direction, Square>()
            {
                { Direction.Up, new Square(this, default) },
                { Direction.Down, new Square(this, default) },
                { Direction.Right, new Square(this, default) },
                { Direction.Left, new Square(this, default) },
            };

            Dictionary<Direction, IReadOnlySquare[,]> directions = new Dictionary<Direction, IReadOnlySquare[,]>();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (dir == Direction.None) continue;
                directions[dir] = new IReadOnlySquare[,]
                {
                    { new Square(this, new ColorChar(ConsoleColor.Black, 'X')), new Square(this, new ColorChar(ConsoleColor.Black, 'X')), Eyes[dir] },
                    { new Square(this, new ColorChar(ConsoleColor.Red, 'O')), new Square(this, new ColorChar(ConsoleColor.Black, 'X')), Mouth[dir] },
                    { BackLeg[dir], SexColorChar, FrontLeg[dir] }
                };
            }
            Body = new Directional2DArray<IReadOnlySquare>(directions[Direction.Up], directions[Direction.Down], directions[Direction.Right], directions[Direction.Left]);

            AnimalHitBox = new AnimalHitBox(this, 3, x, y);
            AnimalHitBox.OnMoveEvent += Animate;
        }

        public abstract void Think();
        public abstract void Animate();

        public void Flip()
        {
            Body.Flipped = !Body.Flipped;
            AnimalHitBox.Update();
        }

        public void SetDirection(Direction dir) => Body.Direction = dir;
    }
}
