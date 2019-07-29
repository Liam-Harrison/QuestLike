using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Organs
{
    public class Nerve : GameObject
    {
        public BodyPart source;
        public BodyPart target;
        public bool hanging = false;

        public Nerve() : base("Nerve", "Transmits signals to bodyparts.", "", new string[] { "nerve", "nervous system", "signal" })
        {
        }

        public Nerve(string name, string[] ids) : base(name, ids)
        {
        }

        public Nerve(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Nerve(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public override string Examine
        {
            get
            {
                return base.Examine + "\n" + "From".Pad() + source.Name + "\n" + "To".Pad() + target.Name;
            }
        }

        public void DetatchFromTarget()
        {
            target = null;
            hanging = true;
        }

        public void DetatchFromSource()
        {
            source = null;
            hanging = true;
        }

        public void Attatch(BodyPart source, BodyPart target)
        {
            hanging = false;
            this.source = source;
            this.target = target;
        }
    }
}
