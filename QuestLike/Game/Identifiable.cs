using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestLike
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class Identifiable
    {
        [JsonProperty]
        private string[] ids;

        public Identifiable()
        {

        }

        public Identifiable(string[] ids)
        {
            this.ids = ids;
        }

        public bool HasID(string id)
        {
            foreach (var i in ids)
            {
                if (id == i) return true;
            }
            return false;
        }
    }
}
