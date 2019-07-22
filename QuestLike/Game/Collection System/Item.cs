using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike
{
    public class Item : GameObject
    {
        public Item(string name, string[] ids) : base(name, ids)
        {
        }

        public Item(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Item(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }
    }
}
