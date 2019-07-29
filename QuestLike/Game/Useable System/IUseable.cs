using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike
{
    interface IUseable
    {
        void Use(GameObject sender, GameObject target, object[] arguments = null);
    }
}
