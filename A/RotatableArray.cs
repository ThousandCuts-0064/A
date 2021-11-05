using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    interface IReadOnlyRotatable2DArray<T>
    {
        T this[int i, int y] { get; }
    }
    class Rotatable2DArray<T> : IReadOnlyRotatable2DArray<T>
    {
        private T[,] array;
        public Direction Direction { get; set; }
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
                    case Direction.Left: return array[array.Length - i, y];
                    default: throw new Exception();
                }
            }
            set => array[i, y] = value;
        }

        public Rotatable2DArray(int size)
        {
            array = new T[size, size];
        }
    }
}
