using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Combat
{
    class Weapon : Item, IUseable
    {
        public Weapon(string name, string[] ids) : this(name, "", ids)
        {
        }

        public Weapon(string name, string desc, string[] ids) : this(name, "", desc, ids)
        {
        }

        public Weapon(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public virtual void Use(GameObject sender, GameObject target, object[] arguments = null)
        {

        }
    }
}
