using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike
{
    class Eatable : Item
    {
        public Eatable(string name, string[] ids) : base(name, ids)
        {
        }

        public Eatable(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Eatable(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public void OnConsume(Entity entity)
        {

        }
    }
}
