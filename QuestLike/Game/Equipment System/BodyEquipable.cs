﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike
{
    class BodyEquipable : Item
    {
        public BodyEquipable(string name, string[] ids) : base(name, ids)
        {
        }

        public BodyEquipable(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public BodyEquipable(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }
    }
}
