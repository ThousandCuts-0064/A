using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    abstract class HitBox
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Size { get; }
        public int Fat { get; }

        public HitBox(int size, int x, int y)
        {
            Size = size;
            Fat = (size - 1) / 2;
            X = x;
            Y = y;
        }

        public MoveError TryMove(string dir)
        {
            if (!Enum.TryParse(dir, out Direction direction)) return MoveError.Invalid;

            return TryMove(direction);
        }

        public abstract MoveError TryMove(Direction dir);

        public bool FreeSpace(Direction dir)
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

    class SimpleHitBox : HitBox
    {
        public SimpleHitBox(int size, int x, int y) : base(size, x, y)
        {

        }

        public override MoveError TryMove(Direction dir)
        {
            throw new NotImplementedException();
        }
    }

    class AnimalHitBox : HitBox
    {
        public delegate void OnMove();
        public event OnMove OnMoveEvent;
        
        public Direction LastMove { get; protected set; } = Direction.Right;
        public Animal Animal { get; }
        public AnimalHitBox(Animal animal, int size, int x, int y) : base(size, x, y)
        {
            Animal = animal;
        }

        public void Update()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Farm.Field[X - Fat + i, Y - Fat + j] = Animal.ReadBody[i, j];
        }

        public override MoveError TryMove(Direction dir)
        {
            if (LastMove != dir && dir != Direction.None)
            {
                LastMove = dir;
                Animal.SetDirection(LastMove);
                Update();
                return MoveError.None;
            }

            if (!FreeSpace(dir))
            {
                LastMove = Direction.None;
                return MoveError.Blocked;
            }

            switch (dir)
            {
                case Direction.None: return MoveError.None;

                case Direction.Up:
                    LastMove = Direction.Up;
                    Animal.SetDirection(LastMove);
                    Move(dir);
                    X -= 1;
                    return MoveError.None;

                case Direction.Down:
                    LastMove = Direction.Down;
                    Animal.SetDirection(LastMove);
                    Move(dir);
                    X += 1;
                    return MoveError.None;


                case Direction.Right:
                    LastMove = Direction.Right;
                    Animal.SetDirection(LastMove);
                    Move(dir);
                    Y += 1;
                    return MoveError.None;


                case Direction.Left:
                    LastMove = Direction.Left;
                    Animal.SetDirection(LastMove);
                    Move(dir);
                    Y -= 1;
                    return MoveError.Blocked;

                default:
                    LastMove = Direction.None;
                    return MoveError.Invalid;
            }
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
                            Farm.Field[X - Fat + j - 1, Y - Fat + i] = Animal.ReadBody[j, i];
                        Farm.Field[X + Fat, Y - Fat + i] = Farm.Empty;
                    }
                    break;

                case Direction.Down:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X + Fat - j + 1, Y - Fat + i] = Animal.ReadBody[Size - Fat - j, i];
                        Farm.Field[X - Fat, Y - Fat + i] = Farm.Empty;
                    }
                    break;

                case Direction.Right:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X - Fat + i, Y + Fat - j + 1] = Animal.ReadBody[i, Size - Fat - j];
                        Farm.Field[X - Fat + i, Y - Fat] = Farm.Empty;
                    }
                    break;

                case Direction.Left:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X - Fat + i, Y - Fat + j - 1] = Animal.ReadBody[i, j];
                        Farm.Field[X - Fat + i, Y + Fat] = Farm.Empty;
                    }
                    break;
            }
        }
    }
}
