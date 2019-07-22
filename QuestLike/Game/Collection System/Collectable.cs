using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike
{
    public class Collectable: Identifiable
    {
        public Collection container;

        public Collection Collection
        {
            get
            {
                return container;
            }
        }

        public Collectable() : base(new string[] { })
        {

        }

        public Collectable(string[] ids) : base(ids)
        {

        }

    }
}
