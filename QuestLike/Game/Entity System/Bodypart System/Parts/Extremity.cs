﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Organs
{
    class Extremity : BodyPart
    {
        public Extremity(string name, string[] ids) : base(name, ids)
        {
        }

        public Extremity(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Extremity(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }
    }
}
