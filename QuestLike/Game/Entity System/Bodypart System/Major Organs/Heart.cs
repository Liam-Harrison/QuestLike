using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Organs
{
    class Heart : BodyPart
    {
        public float lowerHeartPressure;
        public float maximumHeartPressure;

        public Heart() : this("Heart", new string[] { "heart" })
        {
            Generate();
        }

        public Heart(string name, string[] ids) : this(name, "The Heart pumps providing blood pressure for all the bodyparts that are connected to it.\nHowever blood pressure diminishes over distance and the heart pumps less when weakened.", ids)
        {
            Generate();
        }

        public Heart(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Generate();
        }

        public Heart(string name, string desc, string[] ids) : this(name, "", desc, ids)
        {
            Generate();
        }

        private void Generate()
        {
            bloodData.hyperBloodLevel = 750;
        }

        public override void SetBloodPressure(float amount)
        {
            return; // Dont let the hearts blood pressure be affected by other organs - It is where blood pressure is created.
        }

        private Random random = new Random();

        public override void Update()
        {
            float multiplier = 0;
            if (damage == 0) multiplier = (float)random.NextDouble();
            else multiplier = Utilities.Clamp((float)random.NextDouble() - NormalizedDamage + (0.5f * InversedNormalizedDamage), 0, 1);
            bloodData.bloodPressure = Utilities.GetBetweenValues(lowerHeartPressure, maximumHeartPressure, multiplier) / maximumHeartPressure;

            base.Update();
        }
    }
}
