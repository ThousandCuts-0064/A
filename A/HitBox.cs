using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    

    class HitBox
    {
        public delegate void OnMove();
        public event OnMove OnMoveEvent;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Size { get; }
        public int Fat { get; }
        public Direction LastMove { get; protected set; } = Direction.Right;
        public Animal Animal { get; }
        public HitBox(Animal animal, int size, int x, int y)
        {
            Animal = animal;
            Size = size;
            Fat = (size - 1) / 2;
            X = x;
            Y = y;
        }

        public void Update()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Farm.Field[X - Fat + i, Y - Fat + j] = Animal.ReadBody[i, j].Value;
        }

        public MoveError TryMove(Direction dir)
        {
            if (LastMove != dir && dir != Direction.None)
            {
                LastMove = dir;
                Animal.SetDirection(LastMove);
                Update();
                return MoveError.None;
            }

            switch (dir)
            {
                case Direction.None: return MoveError.None;

                case Direction.Up:

                    if (X <= Fat)
                    {
                        LastMove = Direction.None;
                        return MoveError.Blocked;
                    }

                    LastMove = Direction.Up;
                    Animal.SetDirection(LastMove);
                    Move(dir);
                    X -= 1;
                    return MoveError.None;

                case Direction.Down:
                    if (X >= Farm.Size - 1 - Fat)
                    {
                        LastMove = Direction.None;
                        return MoveError.Blocked;
                    }

                    LastMove = Direction.Down;
                    Animal.SetDirection(LastMove);
                    Move(dir);
                    X += 1;
                    return MoveError.None;


                case Direction.Right:
                    if (Y >= Farm.Size - 1 - Fat)
                    {
                        LastMove = Direction.None;
                        return MoveError.Blocked;
                    }

                    LastMove = Direction.Right;
                    Animal.SetDirection(LastMove);
                    Move(dir);
                    Y += 1;
                    return MoveError.None;


                case Direction.Left:
                    if (Y <= Fat)
                    {
                        LastMove = Direction.None;
                        return MoveError.Blocked;
                    }

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

        public MoveError TryMove(string dir)
        {
            if (!Enum.TryParse(dir, out Direction direction)) return MoveError.Invalid;

            return TryMove(direction);
        }

        public void Move(Direction dir)
        {
            OnMoveEvent.Invoke();
            switch (dir)    
            {
                case Direction.Up:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X - Fat + j - 1, Y - Fat + i] = Animal.ReadBody[j, i].Value;
                        Farm.Field[X + Fat, Y - Fat + i] = Farm.Empty;
                    }
                    break;

                case Direction.Down:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X + Fat - j + 1, Y - Fat + i] = Animal.ReadBody[Size - Fat - j, i].Value;
                        Farm.Field[X - Fat, Y - Fat + i] = Farm.Empty;
                    }
                    break;

                case Direction.Right:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X - Fat + i, Y + Fat - j + 1] = Animal.ReadBody[i, Size - Fat - j].Value;
                        Farm.Field[X - Fat + i, Y - Fat] = Farm.Empty;
                    }
                    break;

                case Direction.Left:
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                            Farm.Field[X - Fat + i, Y - Fat + j - 1] = Animal.ReadBody[i, j].Value;
                        Farm.Field[X - Fat + i, Y + Fat] = Farm.Empty;
                    }
                    break;
            }
        }
    }
}
