using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Organs
{
    class CyberHand : CyberneticBodyPart, IHoldable
    {
        private HoldableManager holdableManager;
        public CyberHand(string name, string[] ids) : base(name, ids)
        {
            holdableManager = new HoldableManager(this);
            Generate();
        }

        public CyberHand(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            holdableManager = new HoldableManager(this);
            Generate();
        }

        public CyberHand(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            holdableManager = new HoldableManager(this);
            Generate();
        }

        public bool IsHoldingItem => ((IHoldable)holdableManager).IsHoldingItem;

        public Item HoldingItem => ((IHoldable)holdableManager).HoldingItem;

        public bool GetHoldingSafe(out Item item)
        {
            return ((IHoldable)holdableManager).GetHoldingSafe(out item);
        }

        public Item PutItem(Item item)
        {
            return ((IHoldable)holdableManager).PutItem(item);
        }

        public Item SwitchItems(Item item)
        {
            return ((IHoldable)holdableManager).SwitchItems(item);
        }

        public Item TakeHoldingItem()
        {
            return ((IHoldable)holdableManager).TakeHoldingItem();
        }

        void Generate()
        {
            var thumb = AttachCyberneticBodyPart(new CyberneticBodyPart("Cybernetic Thumb", "Finger", "", new string[] { "finger", "thumb", "cybernetic", "cybernetics", "cyber finger", "cyber thumb", "cybernetic thumb" }));

            var index = AttachCyberneticBodyPart(new CyberneticBodyPart("Cybernetic  Finger", "Finger", "", new string[] { "finger", "index", "cybernetic", "cybernetics", "cyber finger", "cyber index", "cybernetic index",
                "cyber index finger", "cybernetic index finger" }));

            var middle = AttachCyberneticBodyPart(new CyberneticBodyPart("Cybernetic  Finger", "Finger", "", new string[] { "finger", "middle", "cybernetic", "cybernetics", "cyber finger", "cyber middle finger", "cybernetic middle finger" }));

            var ring = AttachCyberneticBodyPart(new CyberneticBodyPart("Cybernetic  Finger", "Finger", "", new string[] { "finger", "ring", "cybernetic", "cybernetics", "cyber finger", "cyber ring finger", "cybernetic ring", "cybernetic ring finger" }));

            var pinky = AttachCyberneticBodyPart(new CyberneticBodyPart("Cybernetic  Finger", "Finger", "", new string[] { "finger", "pinky", "cybernetic", "cybernetics", "cyber finger", "cyber pinky", "cybernetic pinky" }));

            AddCyberConnection(new CyberneticConnection(), thumb);
            AddCyberConnection(new CyberneticConnection(), index);
            AddCyberConnection(new CyberneticConnection(), middle);
            AddCyberConnection(new CyberneticConnection(), ring);
            AddCyberConnection(new CyberneticConnection(), pinky);

            energyProduced = 12;
            energyRequired = 0;
        }
    }
}
