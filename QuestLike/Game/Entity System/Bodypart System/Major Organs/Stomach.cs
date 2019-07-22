using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Stomach : BodyPart
    {
        public Stomach() : base("Stomach", "Breaks down edible and drinkable substances.", "Breaks down organic subtances that can be used for energy.", new string[] { "stomach" })
        {
            Generate();
        }

        public Stomach(string name, string[] ids) : base(name, ids)
        {
            Generate();
        }

        public Stomach(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            Generate();
        }

        public Stomach(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Generate();
        }

        public override string Examine
        {
            get
            {
                string text = "";

                text += gameobjectString;

                text += specifitcationString;

                if (GetCollection<Eatable>().Objects.Length == 0) text += "\n\nThe stomach is empty.";
                else
                {
                    var length = GetCollection<Eatable>().Objects.Length;
                    text += "\n\nThere " + ((length == 1) ? "is " : "are ") + length + ((length == 1) ? " object" : " objects") + " in the stomach.";
                    foreach (var item in GetCollection<Eatable>().Objects)
                    {
                        text += "\n - " + item.Name;
                        if (item.ShortDescription != "") text += " - " + item.ShortDescription;
                    }
                }

                text += attatchmentString;

                text += vesselString;

                text += effector.Examine;

                return text;
            }
        }

        public override void Update()
        {


            base.Update();
        }

        private void Generate()
        {
            AddCollection<Eatable>();

            var parts = GetCollection<BodyPart>();



        }
    }
}
