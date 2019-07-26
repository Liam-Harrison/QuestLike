using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Hand : Extremity, IHoldable
    {
        private HoldableManager holdableManager;

        public Hand(string name, string[] ids) : base(name, ids)
        {
            holdableManager = new HoldableManager(this);
            Generate();
        }

        public Hand(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            holdableManager = new HoldableManager(this);
            Generate();
        }

        public Hand(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
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

        private void Generate()
        {
            var carpal = AttatchBodyPart(new Bone("Carpal", "Minor hand bone", "", new string[] { "bone", "carpal", "carpals", "hand bone", "hand bones" })
            { bloodData = new BloodData() { hypoBloodLevel = 5, hyperBloodLevel = 20, oxygenUse = 0.1f } });

            var metacarpal = AttatchBodyPart(new Bone("Metacarpal", "Minor hand bone", "", new string[] { "bone", "carpal", "metacarpal", "meta carapl", "metacarpals", "meta carpals", "carpals", "hand bone", "hand bones" })
            { bloodData = new BloodData() { hypoBloodLevel = 5, hyperBloodLevel = 20, oxygenUse = 0.1f } });

            var thumb = AttatchBodyPart(new BodyPart("Thumb", "Finger", "", new string[] { "finger", "thumb" })
            { bloodData = new BloodData() { hypoBloodLevel = 5, hyperBloodLevel = 20, oxygenUse = 0.1f } });

            var index = AttatchBodyPart(new BodyPart("Index Finger", "", "", new string[] { "finger", "index" })
            { bloodData = new BloodData() { hypoBloodLevel = 5, hyperBloodLevel = 20, oxygenUse = 0.1f } });

            var middle = AttatchBodyPart(new BodyPart("Middle Finger", "", "", new string[] { "finger", "middle" })
            { bloodData = new BloodData() { hypoBloodLevel = 5, hyperBloodLevel = 20, oxygenUse = 0.1f } });

            var ring = AttatchBodyPart(new BodyPart("Ring Finger", "", "", new string[] { "finger", "ring" })
            { bloodData = new BloodData() { hypoBloodLevel = 5, hyperBloodLevel = 20, oxygenUse = 0.1f } });

            var pinky = AttatchBodyPart(new BodyPart("Pinky Finger", "", "", new string[] { "finger", "pinky" })
            { bloodData = new BloodData() { hypoBloodLevel = 5, hyperBloodLevel = 20, oxygenUse = 0.1f } });

            AddVessel(new SmallArtery(), carpal);
            AddVessel(new SmallArtery(), metacarpal);
            AddVessel(new SmallArtery(), thumb);
            AddVessel(new SmallArtery(), index);
            AddVessel(new SmallArtery(), middle);
            AddVessel(new SmallArtery(), ring);
            AddVessel(new SmallArtery(), pinky);

            AddNerve(new Nerve(), carpal);
            AddNerve(new Nerve(), metacarpal);
            AddNerve(new Nerve(), thumb);
            AddNerve(new Nerve(), index);
            AddNerve(new Nerve(), middle);
            AddNerve(new Nerve(), ring);
            AddNerve(new Nerve(), pinky);

            carpal.AddVessel(new SmallVein(), this);
            carpal.AddVessel(new SmallCapillary(), this);

            metacarpal.AddVessel(new SmallVein(), this);
            metacarpal.AddVessel(new SmallCapillary(), this);

            thumb.AddVessel(new SmallVein(), this);
            thumb.AddVessel(new SmallCapillary(), this);

            index.AddVessel(new SmallVein(), this);
            index.AddVessel(new SmallCapillary(), this);

            middle.AddVessel(new SmallVein(), this);
            middle.AddVessel(new SmallCapillary(), this);

            ring.AddVessel(new SmallVein(), this);
            ring.AddVessel(new SmallCapillary(), this);

            pinky.AddVessel(new SmallVein(), this);
            pinky.AddVessel(new SmallCapillary(), this);

            carpal.PrefillWithBlood();
            metacarpal.PrefillWithBlood();
            thumb.PrefillWithBlood();
            index.PrefillWithBlood();
            middle.PrefillWithBlood();
            ring.PrefillWithBlood();
            pinky.PrefillWithBlood();
        }
    }
}
