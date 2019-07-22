using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Organs;

namespace ZorkLike.Effects
{
    class Reverse : Effect
    {
        public Reverse() : base("Reverse", "Changes the properties of select objects.", "", new string[] { "reverse", "spell" })
        {
            AddAction<Lung>(OnLungAdded, OnLungUpdate, OnLungRemoved);
            uses = 5;
        }

        public void OnLungAdded(GameObject part)
        {
            Console.WriteLine("Reverse began on " + part.Name);
            var lung = part as Lung;
            lung.conversionRate *= -1;
        }

        public void OnLungUpdate(GameObject part)
        {

        }

        public void OnLungRemoved(GameObject part)
        {
            Console.WriteLine("Reverse removed on " + part.Name);
            var lung = part as Lung;
            lung.conversionRate *= -1;
        }
    }
}
