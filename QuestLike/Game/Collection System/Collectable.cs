using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestLike
{
    
    public class Collectable: Identifiable
    {
        
        public ICollection container;

        
        public ICollection Collection
        {
            get
            {
                return container;
            }
        }

        public Collectable() : base()
        {

        }

        public Collectable(string[] ids) : base(ids)
        {

        }

    }
}
