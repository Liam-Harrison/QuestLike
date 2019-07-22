using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Organs;

namespace ZorkLike.Entities
{
    class Human : Humanoid
    {
        public Human(string name, string[] ids) : base(name, ids)
        {
            var parts = GetCollection<BodyPart>();
        }
    }
}
