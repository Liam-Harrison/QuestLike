using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Arm : Extremity
    {
        public Arm(string name, string[] ids) : base(name, ids)
        {
            Generate();
        }

        public Arm(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            Generate();
        }

        public Arm(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Generate();
        }

        private void Generate()
        {

        }
    }
}
