using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Lower_Leg : Extremity
    {
        public Lower_Leg(string name, string[] ids) : base(name, ids)
        {
            Generate();
        }

        public Lower_Leg(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            Generate();
        }

        public Lower_Leg(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Generate();
        }

        void Generate()
        {
            var tibia = AttatchBodyPart(new Bone("Tibia", "Major lower leg bone", new string[] { "bone", "leg bone", "leg bones", "tibia",
                "lower leg bone", "lower leg bones" }));
            AddVessel(new Artery("Femoral Artery"), tibia);
            tibia.AddVessel(new Vein(), this);
            tibia.AddVessel(new Capillary(), this);
            AddNerve(new Nerve(), tibia);
            tibia.PrefillWithBlood();

            var fibia = AttatchBodyPart(new Bone("Fibia", "Major lower leg bone", new string[] { "bone", "leg bone", "leg bones", "fibia",
                "lower leg bone", "lower leg bones" }));
            AddVessel(new Artery("Femoral Artery"), fibia);
            fibia.AddVessel(new Vein(), this);
            fibia.AddVessel(new Capillary(), this);
            AddNerve(new Nerve(), fibia);
            fibia.PrefillWithBlood();

            var tibalis = AttatchBodyPart(new Muscle("Tibialis Anterior", "Major lower leg muscle", "", new string[] { "muscle", "tibialis",
                "tibialis anterior", "lower leg muscle", "lower leg muscles" }));
            AddVessel(new Artery("Femoral Artery"), tibalis);
            tibalis.AddVessel(new Vein(), this);
            tibalis.AddVessel(new Capillary(), this);
            AddNerve(new Nerve(), tibalis);
            tibalis.PrefillWithBlood();
        }
    }
}
