using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    abstract class Environment : FarmObject
    {
        protected SimpleHitBox SimpleHitBox { get; }
        public override HitBox HitBox => SimpleHitBox;
        protected Environment(int x, int y) : base(x, y)
        {
            SimpleHitBox = new SimpleHitBox(1, x, y);
        }
    }
}
