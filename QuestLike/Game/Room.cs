using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike
{
    class Room : GameObject
    {
        public Room() : base("The Room", new string[] { "room", "space" })
        {
            locator.FreezeSearchesBelow = false;
            AddCollection<Entity>();
            AddCollection<GameObject>();
        }

        public Room(string name) : base(name, new string[] { "room", "space" })
        {
            locator.FreezeSearchesBelow = false;
            AddCollection<Entity>();
            AddCollection<GameObject>();
        }

        public Room(string[] ids) : base("The Room", ids)
        {
            locator.FreezeSearchesBelow = false;
            AddCollection<Entity>();
            AddCollection<GameObject>();
        }

        public Room(string name, string[] ids) : base(name, ids)
        {
            locator.FreezeSearchesBelow = false;
            AddCollection<Entity>();
            AddCollection<GameObject>();
        }

        public override void Update()
        {
            base.Update();

            foreach (var entity in GetCollection<Entity>().GetAllObjects()) entity.Update();
        }
    }
}
