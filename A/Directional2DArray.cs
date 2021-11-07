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
        bool Flipped { get; }
    }
    class Directional2DArray<T> : IReadOnlyDirectional2DArray<T>
    {
        private T[,] array;
        public Direction Direction { get; set; } = Direction.Right;
        public bool Flipped { get; set; }
        public T this[int i, int y]
        {
            get
            {
                switch (Direction)
                {
                    case Direction.Up: return array[i, y];
                    case Direction.Down: return array[i, y];
                    case Direction.Right: return array[i, y];
                    case Direction.Left: return array[i, array.GetLength(1) - 1 - y];
                    default: throw new Exception();
                }
            }
            set => array[i, y] = value;
        }

        public Directional2DArray(T[,] array)
        {
            this.array = array;
        }
    }
}
