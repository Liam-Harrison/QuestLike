using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;

namespace QuestLike.Organs
{
    class Chest : Extremity
    {
        public Chest(string name, string[] ids) : base(name, ids)
        {
        }

        public Chest(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Chest(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }
    }
}
