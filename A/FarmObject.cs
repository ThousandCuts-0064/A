﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A
{
    abstract class FarmObject
    {
        public abstract HitBox HitBox { get; }

        public FarmObject(int x, int y)
        {
            
        }
    }
}
