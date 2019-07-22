using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Upper_Arm : Extremity
    {
        public Upper_Arm() : base("Upper Arm", new string[] { "arm", "upper arm" })
        {
            Generate();
        }

        public Upper_Arm(string name) : base(name, new string[] { "arm", "upper arm" })
        {
            Generate();
        }

        public Upper_Arm(string name, string[] ids) : base(name, ids)
        {
            Generate();
        }

        public Upper_Arm(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            Generate();
        }

        public Upper_Arm(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Generate();
        }

        private void Generate()
        {
            bloodData.hyperBloodLevel = 50;
            var parts = GetCollection<BodyPart>();

            // Bones
            var humerus = AttatchBodyPart(new Bone("Humerus", "Major upper arm bone", "", new string[] { "bone", "humerus", "arm bone", "arm bones" }));
            AddVessel(new Artery(), humerus);
            AddNerve(new Nerve(), humerus);
            humerus.AddVessel(new Capillary(), this);
            humerus.AddVessel(new Vein(), this);
            humerus.PrefillWithBlood();

            // Muscles
            var muscle = AttatchBodyPart(new Muscle("Bicep", "Major upper arm muscle", "", new string[] { "muscle", "bicep", "bicep muscle", "upper arm muscle", "upper arm muscles", "arm muscle", "arm muscles" }));
            AddVessel(new Artery("Brachial Artery"), muscle);
            AddNerve(new Nerve(), muscle);
            muscle.AddVessel(new Vein(), this);
            muscle.AddVessel(new Capillary(), this);
            muscle.PrefillWithBlood();
        }
    }
}
