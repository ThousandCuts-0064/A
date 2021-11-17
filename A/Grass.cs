using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    class Grass : Environment
    {
        public bool Mature { get; }
        public Grass(int x, int y) : base(x, y)
        {

        }
    }
}
