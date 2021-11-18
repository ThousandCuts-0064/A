using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    abstract class Body
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Size { get; }
        public int Fat { get; }

        public Body(int size, int x, int y)
        {
            Size = size;
            Fat = (size - 1) / 2;
            X = x;
            Y = y;
        }

        public abstract MoveError TryMove(Direction dir);
        public abstract void Update();

        public MoveError TryMove(string dir)
        {
            if (!Enum.TryParse(dir, out Direction direction)) return MoveError.Invalid;

            return TryMove(direction);
        }

        protected bool FreeSpace(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    if (X - Fat - 1 < 0) return false;
                    for (int i = 0; i < Size; i++)
                        if (Farm.Field[X - Fat - 1, Y - Fat + i].FarmObject != null)
                            return false; 
                    break;

                case Direction.Down:
                    if (X + Fat + 1 > Farm.Size - 1) return false;
                    for (int i = 0; i < Size; i++)
                        if (Farm.Field[X + Fat + 1, Y - Fat + i].FarmObject != null)
                            return false;
                    break;

                case Direction.Right:
                    if (Y + Fat + 1 > Farm.Size - 1) return false;
                    for (int i = 0; i < Size; i++)
                        if (Farm.Field[X - Fat + i, Y + Fat + 1].FarmObject != null)
                            return false;
                    break;

                case Direction.Left:
                    if (Y - Fat - 1 < 0) return false;
                    for (int i = 0; i < Size; i++)
                        if (Farm.Field[X - Fat + i, Y - Fat - 1].FarmObject != null)
                            return false; 
                    break;
            }

            return true;
        }
    }

    class StaticBody : Body
    {
        public StaticBody(int size, int x, int y) : base(size, x, y)
        {

        }

        public override MoveError TryMove(Direction dir)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }

    class AnimalBody : Body
    {
        private readonly Directional2DArray<IReadOnlySquare> dirArr;
        public delegate void OnMove();
        public event OnMove OnMoveEvent;
        public Direction LastMove { get; protected set; } = Direction.Right;
        public Animal Animal { get; }
        public AnimalBody(Animal animal, Dictionary<Direction, IReadOnlySquare[,]> DirDic, int size, int x, int y) : base(size, x, y)
        {
            dirArr = new Directional2DArray<IReadOnlySquare>(DirDic);
            Animal = animal;
        }

        public override void Update()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Farm.Field[X - Fat + i, Y - Fat + j] = dirArr[i, j];
        }

        public void Flip()
        {
            dirArr.Flipped = !dirArr.Flipped;
            Update();
        }

        public void SetDirection(Direction dir) => dirArr.Direction = dir;

        public override MoveError TryMove(Direction dir)
        {
            if (LastMove != dir && dir != Direction.None)
            {
                LastMove = dir;
                dirArr.Direction = LastMove;
                Update();
                return MoveError.None;
            }

            if (!FreeSpace(dir))
            {
                LastMove = Direction.None;
                return MoveError.Blocked;
            }

            LastMove = dir;
            dirArr.Direction = LastMove;
            Move(dir);
            return MoveError.None;
        }

        private void Move(Direction dir)
        {
            OnMoveEvent.Invoke();
            switch (dir)    
            {
                case Direction.Up:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X - Fat + j - 1, Y - Fat + i] = dirArr[j, i];
                        Farm.Field[X + Fat, Y - Fat + i] = Farm.Empty;
                    }
                    X -= 1;
                    break;

                case Direction.Down:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X + Fat - j + 1, Y - Fat + i] = dirArr[Size - Fat - j, i];
                        Farm.Field[X - Fat, Y - Fat + i] = Farm.Empty;
                    }
                    X += 1;
                    break;

                case Direction.Right:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X - Fat + i, Y + Fat - j + 1] = dirArr[i, Size - Fat - j];
                        Farm.Field[X - Fat + i, Y - Fat] = Farm.Empty;
                    }
                    Y += 1;
                    break;

                case Direction.Left:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X - Fat + i, Y - Fat + j - 1] = dirArr[i, j];
                        Farm.Field[X - Fat + i, Y + Fat] = Farm.Empty;
                    }
                    Y -= 1;
                    break;
            }
        }
    }
}
