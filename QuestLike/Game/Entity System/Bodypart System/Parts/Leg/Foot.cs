using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Foot : Extremity
    {
        public Foot(string name, string[] ids) : base(name, ids)
        {
            Generate();
        }

        public Foot(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            Generate();
        }

        public Foot(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Generate();
        }

        void Generate()
        {
            var calcaneus = AttatchBodyPart(new Bone("Calcaneus", "Minor foot bone", "", new string[] { "bone", "foot bone", "calcaneus", "feet bones" }));
            AddNerve(new Nerve(), calcaneus);
            AddVessel(new SmallArtery(), calcaneus);
            calcaneus.AddVessel(new SmallVein(), this);
            calcaneus.AddVessel(new SmallCapillary(), this);
            calcaneus.PrefillWithBlood();

            var metatarsal = AttatchBodyPart(new Bone("Metatarsals", "Minor foot bones", "", new string[] { "bone", "foot bone", "metatarsals", "metatarsal", "feet bones" }));
            AddNerve(new Nerve(), metatarsal);
            AddVessel(new Artery(), metatarsal);
            metatarsal.AddVessel(new Vein(), this);
            metatarsal.AddVessel(new Capillary(), this);
            metatarsal.PrefillWithBlood();

            var phalanges = metatarsal.AttatchBodyPart(new Bone("Phalanges", "Minor foot bones", "", new string[] { "bone", "foot bone", "phalanges", "phalange", "feet bones" }));
            metatarsal.AddNerve(new Nerve(), phalanges);
            metatarsal.AddVessel(new SmallArtery(), phalanges);
            phalanges.AddVessel(new SmallVein(), metatarsal);
            phalanges.AddVessel(new SmallCapillary(), metatarsal);
            phalanges.PrefillWithBlood();

            var quadratus = AttatchBodyPart(new Muscle("Quadratus plantae", "Minor foot muscle", "", new string[] { "muscle", "foot muscle", "quadratus", "quadratus plantae" }));
            AddNerve(new Nerve(), quadratus);
            AddVessel(new SmallArtery(), quadratus);
            quadratus.AddVessel(new SmallVein(), this);
            quadratus.AddVessel(new SmallCapillary(), this);
            quadratus.PrefillWithBlood();
        }

    }
}
