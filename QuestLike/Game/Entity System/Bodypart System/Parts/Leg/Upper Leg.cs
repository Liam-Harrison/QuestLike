using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Organs
{
    class Upper_Leg : Extremity
    {
        public Upper_Leg(string name, string[] ids) : base(name, ids)
        {
            Generate();
        }

        public Upper_Leg(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            Generate();
        }

        public Upper_Leg(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Generate();
        }

        void Generate()
        {
            var femur = AttatchBodyPart(new Bone("Femur", "Major upper leg bone", "", new string[] { "femur", "bone", "leg bone", "leg bones",
                "upper leg bone", "upper leg bones" }));

            var quads = AttatchBodyPart(new Muscle("Quadroceps", "Major upper leg muscle", "", new string[] { "quad", "quadrocep", "quadroceps", "leg muscle",
                "leg muscles", "upper leg muscle", "upper leg muscles" }));

            AddVessel(new Artery(), femur);
            AddNerve(new Nerve(), femur);
            femur.AddVessel(new Vein(), this);
            femur.AddVessel(new Capillary(), this);
            femur.PrefillWithBlood();

            AddVessel(new Artery(), quads);
            AddNerve(new Nerve(), quads);
            quads.AddVessel(new Vein(), this);
            quads.AddVessel(new Capillary(), this);
            quads.PrefillWithBlood();
        }
    }
}
