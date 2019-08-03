using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using QuestLike.Entities;

namespace QuestLike
{
    class Player : PartialCyberHumanoid
    {

        public Player() : base("Myself", new string[] { "me", "player", "myself" })
        {
            screenChar = (char)1;
            position = new Microsoft.Xna.Framework.Point(12, 5);
        }

        public override void Update()
        {
            base.Update();
        }

    }
}
