using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Organs;
using ZorkLike.Entities;

namespace ZorkLike
{
    class Player : PartialCyberHuman
    {

        public Player() : base("Myself", new string[] { "me", "player", "myself" })
        {
        }

        public override void Update()
        {
            base.Update();
        }

    }
}
