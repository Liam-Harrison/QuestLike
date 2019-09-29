using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Combat;

namespace QuestLike.Organs
{
    class Lung : BodyPart
    {
        public float conversionRate;
        public float maximumConversion;

        public Lung(float conversionRate, float maximumConversion) : this("Lung", conversionRate, maximumConversion, new string[] { "lung" })
        {
        }

        public Lung(string name, float conversionRate, float maximumConversion) : this(name, conversionRate, maximumConversion, "", "The Lung processes deoxygenated blood - changing it into useful oxygenated blood.", new string[] { "lung" })
        {
        }

        public Lung(string name, float conversionRate, float maximumConversion, string[] ids) : base(name, "", "The Lung processes deoxygenated blood - changing it into useful oxygenated blood.", ids)
        {
            this.conversionRate = conversionRate;
            this.maximumConversion = maximumConversion;
            Generate();
        }

        public Lung(string name, float conversionRate, float maximumConversion, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            this.conversionRate = conversionRate;
            this.maximumConversion = maximumConversion;
            Generate();
        }

        void Generate()
        {
            bloodData.hyperBloodLevel = 750;
        }

        public override void Update()
        {
            float bloodChange = 0;

            if (conversionRate > 0) bloodChange = Utilities.Clamp(bloodData.deoxygenatedBlood * conversionRate, 0, maximumConversion);
            else bloodChange = -Utilities.Clamp(bloodData.oxygenatedBlood * -conversionRate, 0, maximumConversion);

            bloodData.deoxygenatedBlood -= bloodChange;
            bloodData.oxygenatedBlood += bloodChange;

            base.Update();
        }

        public override string Examine
        {
            get
            {

                string text = "";

                text += Name + ((ShortDescription == "") ? "" : " - " + ShortDescription) + ((Description == "") ? "" : "\n" + Description);

                text += "\n";

                text += specifitcationString;

                if (conversionRate > 0) text += "\n" + "Currently converting".Pad(27) + Utilities.Clamp(bloodData.deoxygenatedBlood * conversionRate, 0, maximumConversion).ToString("0.0") + "ml/turn\n";
                else text += "\n" + "Currently converting".Pad(27) + "-" + Utilities.Clamp(bloodData.oxygenatedBlood * -conversionRate, 0, maximumConversion).ToString("0.0") + "ml/turn\n";

                text += "Maximum can convert".Pad(27) + maximumConversion.ToString("0.0") + "ml/turn";

                text += attatchmentString;

                text += vesselString;

                text += effector.Examine;

                text += interactionString;

                return text;
            }
        }
    }
}
