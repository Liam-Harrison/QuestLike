using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole;
using Console = SadConsole.Console;
using Global = SadConsole.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Input;
using QuestLike;
using Newtonsoft.Json;

namespace QuestLike.Organs
{

    public class CyberneticConnection : Nerve
    {
        public float powerTransfer = 20;
        public float powerFalloff = 0.2f;
        public bool transmitsNerveInformation = true;

        public CyberneticConnection() : base("Cyber Connection", "Connects cybernet parts together - transmiting power and data.", "", new string[] { "connection", "cyberware", "cyber connection", "cyberware connection",
            "connector", "cyberware connector", "cyber connector" })
        {
        }

        public CyberneticConnection(string name, string[] ids) : base(name, ids)
        {
        }

        public CyberneticConnection(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public CyberneticConnection(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }


        public override string Examine
        {
            get
            {
                return base.Examine + "\n" + "From".Pad() + source.Name + "\n" + "To".Pad() + target.Name;
            }
        }
    }


    public class CyberneticBodyPart : BodyPart
    {
        public float energyRequired = 2;
        public float energyProduced = 0;
        public float maxEnergy = 30;
        public float energy = 10;
        public bool passEnergyEvenWhenLow = false;

        private void Setup()
        {
            AddCollection<CyberneticConnection>();
            usesBlood = false;
            bloodData = new BloodData()
            {
                bloodPressureFalloff = 0,
                hypoBloodLevel = 0,
            };
        }

        public CyberneticBodyPart(string name, string[] ids) : base(name, ids)
        {
            Setup();
        }

        public CyberneticBodyPart(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            Setup();
        }

        public CyberneticBodyPart(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            Setup();
        }

        public void AddCyberConnection(CyberneticConnection connection, CyberneticBodyPart target)
        {
            connection.source = this;
            connection.target = target;
            GetCollection<CyberneticConnection>().AddObject(connection);
        }

        public CyberneticBodyPart AttachCyberneticBodyPart(CyberneticBodyPart part)
        {
            part.parent = this;
            GetCollection<BodyPart>().AddObject(part);
            return part;
        }

        public override void Update()
        {
            base.Update();

            energy = Utilities.Clamp(energy + energyProduced, 0, maxEnergy);
            energy = Utilities.Clamp(energy - energyRequired, 0, maxEnergy);

            float toSend = energy - energyRequired;
            if (passEnergyEvenWhenLow) toSend = energy;

            float totalPowerNeeded = GetCollection<CyberneticConnection>().ObjectList.Sum((a) => { return a.powerTransfer; });

            foreach (var connection in GetCollection<CyberneticConnection>().Objects)
            {
                if (connection.target is CyberneticBodyPart && connection.source is CyberneticBodyPart)
                if (passEnergyEvenWhenLow || energy > energyRequired)
                {
                    var target = connection.target as CyberneticBodyPart;
                    float sending = Utilities.Clamp(toSend * (connection.powerTransfer / totalPowerNeeded), 0, connection.powerTransfer);
                    target.energy = Utilities.Clamp(target.energy + sending, 0, target.maxEnergy);
                    energy = Utilities.Clamp(energy - sending, 0, maxEnergy);
                }
            }
        }

        public override string Examine
        {
            get
            {
                string text = "";

                text += Name + ((ShortDescription == "") ? "" : " - " + ShortDescription) + ((Description == "") ? "" : "\n" + Description)
                    + equipedDescription + holdingDescription + inventoryDescription + objectsDescription + "\n";

                if (energyProduced > 0) text += "\nProducing".Pad(28) + energyProduced + " amps/turn";

                if (energyProduced == 0 || energy > 0)
                {
                    text += "\nEnergy ".Pad(28) + energy + " amps";
                    if (energy < energyRequired) text += $" <{Color.Yellow.ToInteger()},>(low energy)@";
                }

                if (energyRequired > 0) text += "\nRequires ".Pad(28) + energy + " amps/turn";

                text +="\nMaximum energy capacity".Pad(28) + maxEnergy + " amps";
                text +="\nHealth".Pad(28) + DamageLevel.ToString();
                text +="\nConnected to singal sender".Pad(28) + (ConnectedToSignalSender ? "Yes" : "No");

                text += attatchmentString;

                text += vesselString;

                text += effector.Examine;

                text += interactionString;

                return text;
            }
        }
    }
}
