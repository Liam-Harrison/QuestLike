using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using QuestLike.Combat;

namespace QuestLike
{
    class Entity : GameObject, IInventory
    {
        private Inventory inventory;

        public Entity(string name, string[] ids) : this(name, "", ids)
        {
        }

        public Entity(string name, string desc, string[] ids) : this(name, "", desc, ids)
        {
        }

        public Entity(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            inventory = new Inventory(this);
            AddCollection<BodyPart>();
            GetCollection<BodyPart>().treeSearchable = false;
            AddCollection<Item>();
        }

        public override void Update()
        {
            base.Update();

            foreach (var i in GetCollection<BodyPart>().GetAllObjects())
            {
                i.Update();
            }
        }

        public override string Examine
        {
            get
            {
                string text = base.Examine;

                return text;
            }
        }

        public Inventory GetInventory => inventory.GetInventory;
    }
}
