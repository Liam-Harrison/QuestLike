using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Head : Extremity
    {

        public Head() : base("Head", "Probably the most important part.", new string[] { "head" })
        {
            bloodData.hypoBloodLevel = 20;
            bloodData.hyperBloodLevel = 60;
            Generate();
        }

        public Head(string name, string[] ids) : base(name, ids)
        {
            bloodData.hypoBloodLevel = 20;
            bloodData.hyperBloodLevel = 60;
            Generate();
        }

        public Head(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            bloodData.hypoBloodLevel = 20;
            bloodData.hyperBloodLevel = 60;
            Generate();
        }

        public Head(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            bloodData.hypoBloodLevel = 20;
            bloodData.hyperBloodLevel = 60;
            Generate();
        }

        public override void Update()
        {
            base.Update();
        }

        private void Generate()
        {
            var skull = AttatchBodyPart(new Bone("Skull", new string[] { "skull", "bone" }));
            AddVessel(new SmallArtery(), skull);
            skull.AddVessel(new SmallVein(), this);
            skull.AddVessel(new SmallCapillary(), this);
            AddNerve(new Nerve(), skull);
            skull.PrefillWithBlood();
        }
    }
}
