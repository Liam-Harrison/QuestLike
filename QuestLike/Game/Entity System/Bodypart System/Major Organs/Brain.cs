using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class Brain : BodyPart
    {
        public Brain() : base("Brain", "Transmits signals to all your bodyparts", "", new string[] { "brain" })
        {
            bloodData.hypoBloodLevel = 20;
            bloodData.hyperBloodLevel = 60;
            Generate();
        }

        public Brain(string name, string[] ids) : base(name, ids)
        {
            bloodData.hypoBloodLevel = 20;
            bloodData.hyperBloodLevel = 60;
            Generate();
        }

        public Brain(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            bloodData.hypoBloodLevel = 20;
            bloodData.hyperBloodLevel = 60;
            Generate();
        }

        public Brain(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            bloodData.hypoBloodLevel = 20;
            bloodData.hyperBloodLevel = 60;
            Generate();
        }

        private void Generate()
        {

        }
    }
}
