using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Organs
{

    public class BloodVessel : GameObject
    {
        public BodyPart source;
        public BodyPart target;
        public float capacity;
        public bool movesOxygenated = true;
        public bool transfersBloodPressure = true;
        public bool hanging = false;

        public BloodVessel() : base("blood vessel", "Transports blood", "", new string[] { "vessel" })
        {
        }

        public BloodVessel(string name, string[] ids) : base(name, ids)
        {
        }

        public BloodVessel(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public BloodVessel(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public override string Examine
        {
            get
            {
                return base.Examine + "\n" + "From".Pad() + source.Name + "\n" + "To".Pad() + target.Name + "\n" + "Capacity".Pad() + capacity.ToString("0.0") + "ml";
            }
        }
    }

#region Arteries
    public class Artery : BloodVessel
    {
        public Artery(string name) : base(name, "Transports blood", "", new string[] { "vessel", "artery" })
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 8;
        }

        public Artery() : base("artery", "Transports blood", "", new string[] { "vessel", "artery" })
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 8;
        }

        public Artery(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 8;
        }

        public Artery(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 8;
        }

        public Artery(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 8;
        }
    }

    public class LargeArtery : Artery
    {
        public LargeArtery() : base("large artery", "Transports blood", "", new string[] { "vessel", "artery" })
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 16;
        }

        public LargeArtery(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 16;
        }

        public LargeArtery(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 16;
        }

        public LargeArtery(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 16;
        }
    }

    public class SmallArtery : Artery
    {
        public SmallArtery() : base("large artery", "Transports blood", "", new string[] { "vessel", "artery" })
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 2;
        }

        public SmallArtery(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 2;
        }

        public SmallArtery(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 2;
        }

        public SmallArtery(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = true;
            capacity = 2;
        }
    }
    #endregion


    public class PulmonaryArtery : BloodVessel
    {
        public PulmonaryArtery() : base("pulmonary artery", "Transports blood", "", new string[] { "vessel", "artery", "blood vessel", "pulmonary artery" })
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 8;
        }

        public PulmonaryArtery(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 8;
        }

        public PulmonaryArtery(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 8;
        }

        public PulmonaryArtery(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 8;
        }
    }

    public class LargePulmonaryArtery : PulmonaryArtery
    {
        public LargePulmonaryArtery() : base("large pulmonary artery", "Transports blood", "", new string[] { "vessel", "artery", "blood vessel", "pulmonary artery", "large pulmonary artery" })
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 16;
        }

        public LargePulmonaryArtery(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 16;
        }

        public LargePulmonaryArtery(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 16;
        }

        public LargePulmonaryArtery(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 16;
        }
    }

    public class SmallPulmonaryArtery : PulmonaryArtery
    {
        public SmallPulmonaryArtery() : base("small pulmonary artery", "Transports blood", "", new string[] { "vessel", "artery", "blood vessel", "pulmonary artery", "small pulmonary artery" })
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 2;
        }

        public SmallPulmonaryArtery(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 2;
        }

        public SmallPulmonaryArtery(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 2;
        }

        public SmallPulmonaryArtery(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = true;
            capacity = 2;
        }
    }


    public class Vein : BloodVessel
    {
        public Vein() : base("vein", "Transports blood", "", new string[] { "vessel", "vein", "blood vessel" })
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 8;
        }

        public Vein(string name) : base(name, "Transports blood", "", new string[] { "vessel", "vein", "blood vessel" })
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 8;
        }

        public Vein(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 8;
        }

        public Vein(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 8;
        }

        public Vein(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 8;
        }
    }


    public class LargeVein : Vein
    {
        public LargeVein() : base("large vein", "Transports blood", "", new string[] { "vessel", "vein", "blood vessel", "large vein" })
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 16;
        }

        public LargeVein(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 16;
        }

        public LargeVein(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 16;
        }

        public LargeVein(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 16;
        }
    }


    public class SmallVein : Vein
    {
        public SmallVein() : base("small vein", "Transports blood", "", new string[] { "vessel", "vein", "blood vessel", "small vein" })
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 2;
        }

        public SmallVein(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 2;
        }

        public SmallVein(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 2;
        }

        public SmallVein(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = false;
            transfersBloodPressure = false;
            capacity = 2;
        }
    }


    public class Capillary : BloodVessel
    {
        public Capillary() : base("capillary", "Transports blood", "", new string[] { "vessel", "capillary", "blood vessel" })
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 8;
        }

        public Capillary(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 8;
        }

        public Capillary(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 8;
        }

        public Capillary(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 8;
        }
    }

    public class LargeCapillary : Capillary
    {
        public LargeCapillary() : base("large capillary", "Transports blood", "", new string[] { "vessel", "capillary", "blood vessel", "large capillary" })
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 16;
        }

        public LargeCapillary(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 16;
        }

        public LargeCapillary(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 16;
        }

        public LargeCapillary(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 16;
        }
    }

    public class SmallCapillary : Capillary
    {
        public SmallCapillary() : base("small capillary", "Transports blood", "", new string[] { "vessel", "capillary", "blood vessel", "small capillary" })
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 2;
        }

        public SmallCapillary(string name, string[] ids) : base(name, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 2;
        }

        public SmallCapillary(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 2;
        }

        public SmallCapillary(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            movesOxygenated = true;
            transfersBloodPressure = false;
            capacity = 2;
        }
    }
}
