using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Organs
{
    class CyberSignalSender : CyberneticBodyPart
    {
        public CyberSignalSender() : base("Cyber Signal Generator", "Generates signals for cyberparts - removing need for brain connections.", "", new string[] { "cyberware", "cyber signal generator",
            "cybernetic signal generator" })
        {
        }

        public CyberSignalSender(string name, string[] ids) : base(name, ids)
        {
        }

        public CyberSignalSender(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public CyberSignalSender(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }
    }
}
