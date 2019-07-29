using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Organs
{
    class Bone : BodyPart
    {
        public float bloodGeneratedPerTurn = 1f;

        public Bone(string name, string[] ids) : base(name, ids)
        {
            bloodData.oxygenUse = 0.25f;
        }

        public Bone(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            bloodData.oxygenUse = 0.25f;
        }

        public Bone(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            bloodData.oxygenUse = 0.25f;
        }

        public override void Update()
        {
            bloodData.deoxygenatedBlood += bloodGeneratedPerTurn;

            base.Update();
        }
    }
}
