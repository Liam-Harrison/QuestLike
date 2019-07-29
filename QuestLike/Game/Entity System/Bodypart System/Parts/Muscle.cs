﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Organs
{
    class Muscle : BodyPart
    {
        public Muscle() : base ("muscle", new string[] { "muscle" })
        {

        }

        public Muscle(string name, string[] ids) : base(name, ids)
        {
        }

        public Muscle(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Muscle(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }
    }
}
