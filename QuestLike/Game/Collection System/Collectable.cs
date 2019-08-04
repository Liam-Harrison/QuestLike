using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestLike
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Collectable: Identifiable
    {
        [JsonProperty(IsReference = true)]
        public ICollection container;

        [JsonIgnore]
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
