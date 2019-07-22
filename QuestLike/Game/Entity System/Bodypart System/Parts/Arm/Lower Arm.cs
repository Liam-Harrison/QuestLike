using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Lower_Arm : Extremity
    {
        public Lower_Arm() : base("Lower Arm", new string[] { "arm", "lower arm" })
        {
            Generate();
        }

        public Lower_Arm(string name) : base(name, new string[] { "arm", "lower arm" })
        {
            Generate();
        }

        public Lower_Arm(string name, string[] ids) : base(name, ids)
        {
            Generate();
        }

        public Lower_Arm(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            Generate();
        }

        public Lower_Arm(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Generate();
        }

        private void Generate()
        {
            var parts = GetCollection<BodyPart>();

            var radius = AttatchBodyPart(new Bone("Radius", "Major lower arm bone", "", new string[] { "bone", "radius", "arm bone", "arm bones" }));
            var ulna = AttatchBodyPart(new Bone("Ulna", "Major lower arm bone", "", new string[] { "bone", "ulna", "arm bone", "arm bones" }));
            var brachioradialis = AttatchBodyPart(new Muscle("Brachio-Radialis", "Major lower arm muscle", "", new string[] { "muscle", "brachioradialis", "brachio radialis", "brachioradialis muscle", "lower arm muscle", "lower arm muscles", "arm muscle", "arm muscles" }));
            var flexor = AttatchBodyPart(new Muscle("Flexor Carpi Radialis", "Major lower arm muscle", "", new string[] { "muscle", "flexor carpi radialis", "flexor carpi radialis muscle", "lower arm muscle", "lower arm muscles", "arm muscle", "arm muscles" }));

            AddVessel(new Artery("Ulnar Bone Artery"), ulna);
            AddVessel(new Artery("Ulnar Muscle Artery"), flexor);
            AddVessel(new Artery("Radial Bone Artery"), radius);
            AddVessel(new Artery("Radial Muscle Artery"), brachioradialis);
            AddNerve(new Nerve(), ulna);
            AddNerve(new Nerve(), flexor);
            AddNerve(new Nerve(), radius);
            AddNerve(new Nerve(), brachioradialis);

            ulna.AddVessel(new Vein("Ulnar Vein"), this);
            flexor.AddVessel(new Vein("Ulnar Vein"), this);
            radius.AddVessel(new Vein("Radial Vein"), this);
            brachioradialis.AddVessel(new Vein("Radial Vein"), this);

            ulna.AddVessel(new Capillary(), this);
            flexor.AddVessel(new Capillary(), this);
            radius.AddVessel(new Capillary(), this);
            brachioradialis.AddVessel(new Capillary(), this);

            radius.PrefillWithBlood();
            ulna.PrefillWithBlood();
            brachioradialis.PrefillWithBlood();
            flexor.PrefillWithBlood();
        }

    }
}
