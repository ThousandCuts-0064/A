using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    interface IReadOnlyDirectional2DArray<T>
    {
        T this[int i, int y] { get; }
        Direction Direction { get; }
        int sizeX { get; }
        int sizeY { get; }
        bool Flipped { get; }
    }
    class Directional2DArray<T> : IReadOnlyDirectional2DArray<T>
    {
        private Dictionary<Direction, T[,]> arrays;
        public Direction Direction { get; set; } = Direction.Right;
        public int sizeX { get; }
        public int sizeY { get; }
        public bool Flipped { get; set; } = false;
        public T this[int i, int y]
        {
            get
            {
                if (Flipped)
                {
                    
                    switch (Direction)
                    {
                        case Direction.Up: return arrays[Direction.Left][y, sizeX - 1 - i];
                        case Direction.Down: return arrays[Direction.Left][y, i];
                        case Direction.Right: return arrays[Direction.Down][sizeX - 1 - i, y];
                        case Direction.Left: return arrays[Direction.Down][sizeX - 1 - i, sizeY - 1 - y];
                        default: throw new Exception();
                    }
                }
                else
                {
                    switch (Direction)
                    {
                        case Direction.Up: return arrays[Direction.Right][sizeY - 1 - y, sizeX - 1 - i];
                        case Direction.Down: return arrays[Direction.Right][sizeY - 1 - y, i];
                        case Direction.Right: return arrays[Direction.Up][i, y];
                        case Direction.Left: return arrays[Direction.Up][i, sizeY - 1 - y];
                        default: throw new Exception();
                    }
                }
            }
            set => arrays[Direction.Up][i, y] = value;
        }

        public Directional2DArray(T[,] up, T[,] down, T[,] right, T[,] left)
        {
            arrays = new Dictionary<Direction, T[,]>()
            {
                {Direction.Up, up},
                {Direction.Down, down},
                {Direction.Right, right},
                {Direction.Left, left},
            };
            sizeX = up.GetLength(0);
            sizeY = up.GetLength(1);
        }
    }
}
