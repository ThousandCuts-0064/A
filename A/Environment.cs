using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    abstract class Environment : FarmObject
    {
        protected StaticBody StaticBody { get; }
        public override Body Body => StaticBody;
        protected Environment(int x, int y) : base(x, y)
        {
            StaticBody = new StaticBody(1, x, y);
        }
    }
}
