using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Organs;

namespace ZorkLike.Command
{
    class DebugCommand : Command
    {

        public DebugCommand()
        {
            keywords = new string[] { };
            usecases = new string[] { "debug look at $", "debug simulate $", "debug check signal to ^ on ^", "debug check signal to my ^", "sever my ^", "sever +" };
            tags = new string[] { "debug" };
            commandName = "Debug";
        }

        public override bool Execute(bool admin)
        {
            if (!Settings.DebugMode)
            {
                GameScreen.PrintLine($"\n<{Color.Red.ToInteger()},>You can only use debugging commands when in debug mode. Goto Options > Debug Mode > On@");
                return false;
            }
            switch (usecaseID)
            {
                case 0:
                    GameObject i = null;
                    Game.LocateSingleObject(args[0], (a, b) => {
                        i = a;

                        if (i == null)
                        {
                            GameScreen.PrintLine("\nCould not find anything like that.");
                        }

                        PrintDetailedBodyPartData(i);

                    });
                    break;
                case 1:

                    Game.LocateSingleObject(args[0], (a, b) => {
                        i = a;

                        if (i != null)
                        {
                            RunSimulation(i);
                        }
                    });

                    break;
                case 2:

                    Game.LocateSingleObject(args[1], (a, b) => {

                        a.LocateSingleObject(args[0], (c, d) => {
                            var item = c;

                            if (item != null && a != null)
                            {
                                if (item is BodyPart)
                                {
                                    var j = item as BodyPart;
                                    if (j.ConnectedToSignalSender) GameScreen.PrintLine("\n" + j.Name + " is connected to a signal sender.");
                                    else GameScreen.PrintLine("\n" + j.Name + " is not connected to a signal sender.");
                                }
                            }
                        });

                    });

                    break;
                case 3:

                    var target = Game.GetPlayer;
                    target.LocateSingleObject(args[0], (a, b) => {
                        var item = a;

                        if (item != null && target != null)
                        {
                            if (item is BodyPart)
                            {
                                var j = item as BodyPart;
                                if (j.ConnectedToSignalSender) GameScreen.PrintLine("\n" + j.Name + " is connected to a signal sender.");
                                else GameScreen.PrintLine("\n" + j.Name + " is not connected to a signal sender.");
                            }
                        }
                    });

                    break;
                case 4:

                    Game.GetPlayer.LocateSingleObjectOfType<BodyPart>(GetArg(0), (part, b) => 
                    {
                        part.SeverPart(Game.GetRoom);
                        GameScreen.PrintLine($"\nSevered the <{Color.Cyan.ToInteger()},look at {part.ID}>{part.Name}@, all blood vessels, nerves and cyberconections were disconnected.");
                    });

                    break;
                case 5:

                    if (int.TryParse(GetArg(0), out int id)) {
                        var part = Game.LocateWithGameID(id);
                        if (part as BodyPart == null) break;
                        var casted = part as BodyPart;
                        casted.SeverPart(Game.GetRoom);
                        GameScreen.PrintLine($"\nSevered the <{Color.Cyan.ToInteger()},look at {part.ID}>{part.Name}@, all blood vessels, nerves and cyberconections were disconnected.");
                    }
                    
                    break;
            }
            return false;
        }

        private static void RunSimulation(GameObject i)
        {
            GameScreen.PrintLine("\nRunning a simulation on " + i.Name + " for 1000 ticks");

            for (int x = 0; x < 1000; x++)
            {
                i.Update();
            }

            GameScreen.PrintLine("Finished simulation - Assembling report");
            int errors = 0;
            foreach (var part in i.LocateObjectsWithType<BodyPart>(false))
            {
                if ((part.bloodData.oxygenatedBlood < part.bloodData.hypoBloodLevel) && part.usesBlood)
                {
                    GameScreen.PrintLine($"\n<{Color.Yellow.ToInteger()},>low oxygenated blood in " + part.Name + "@");
                    GameScreen.PrintLine("Oxygenated Blood".Pad(18) + part.bloodData.oxygenatedBlood.ToString("0.00ml"));
                    GameScreen.PrintLine("Deoxygenated Blood".Pad(18) + part.bloodData.deoxygenatedBlood.ToString("0.00ml"));
                    errors++;
                }

                if ((part.bloodData.oxygenatedBlood > part.bloodData.hyperBloodLevel) && part.usesBlood)
                {
                    GameScreen.PrintLine($"\n<{Color.LightGreen.ToInteger()},>high oxygenated blood in " + part.Name + "@");
                    GameScreen.PrintLine("Oxygenated Blood".Pad(18) + part.bloodData.oxygenatedBlood.ToString("0.00ml"));
                    GameScreen.PrintLine("Deoxygenated Blood".Pad(18) + part.bloodData.deoxygenatedBlood.ToString("0.00ml"));
                }

                if (part.Damage > 0)
                {
                    GameScreen.PrintLine($"\n<{Color.Cyan.ToInteger()},>damage found in " + part.Name + "@");
                    GameScreen.PrintLine("Oxygenated Blood".Pad(18) + part.bloodData.oxygenatedBlood.ToString("0.00ml"));
                    GameScreen.PrintLine("Deoxygenated Blood".Pad(18) + part.bloodData.deoxygenatedBlood.ToString("0.00ml"));
                    GameScreen.PrintLine("Damage".Pad(18) + part.Damage + " (" + part.DamageLevel + ")");
                    errors++;
                }
            }

            if (errors > 0)
            {
                GameScreen.PrintLine($"\n<{Color.Yellow.ToInteger()},>Identified " + errors + " potential issues.@");
            }
            else
            {
                GameScreen.PrintLine($"\n<{Color.LightGreen.ToInteger()},>Found no issues! Stable anatomy achieved!@");
            }
        }

        public void PrintGameObjectData(GameObject i)
        {
            GameScreen.Print("\n" + i.Name);
            if (i.ShortDescription != "") GameScreen.Print(" - " + i.ShortDescription);
            if (i.Description != "") GameScreen.Print("\n" + i.Description);
            GameScreen.Print("\n");
        }

        float totalBlood = 0;
        public void PrintDetailedBodyPartData(GameObject i)
        {
            totalBlood = 0;
            var collection = i.GetCollection<BodyPart>().ObjectList;
            foreach (var j in collection)
            {
                PrintBodyData(j);
            }
            GameScreen.PrintLine("\nTotal blood in system: " + totalBlood.ToString("0.0") + "mL");
        }

        public void PrintBodyData(BodyPart j)
        {
            GameScreen.PrintLine(j.Name.Pad(' ', 22));
            GameScreen.Print(" damage: " + j.Damage.ToString("0").Pad(' ', 5));
            if (j.usesBlood)
            {
                if (j.bloodData.oxygenatedBlood > j.bloodData.hyperBloodLevel) GameScreen.Print(" <4287688336,>oxygen: " + (j.bloodData.oxygenatedBlood.ToString("0.00").PadRight(12, ' ')) + "@");
                else GameScreen.Print(" oxygen: " + (j.bloodData.oxygenatedBlood.ToString("0.00").PadRight(13, ' ')));

                if (j.bloodData.deoxygenatedBlood > 150) GameScreen.Print($" <{Color.Orange.ToInteger()},>deoxygenated: " + (j.bloodData.deoxygenatedBlood.ToString("0.00").PadRight(12, ' ')) + "@");
                else GameScreen.Print(" deoxygenated: " + (j.bloodData.deoxygenatedBlood.ToString("0.00").PadRight(13, ' ')));
            }
            else if (j is CyberneticBodyPart)
            {
                var i = j as CyberneticBodyPart;
                if (i.energy < i.energyRequired) GameScreen.Print($" <{Color.Orange.ToInteger()},>energy: " + i.energy.ToString("0.00") + "/" + (i.energyRequired.ToString("0.00 volts").PadRight(12, ' ')) + "@");
                else GameScreen.Print(" energy: " + i.energy.ToString("0.00") + "/" + (i.energyRequired.ToString("0.00 volts").PadRight(13, ' ')));
            }

            totalBlood += j.TotalBlood;

            foreach (var x in j.Children)
            {
                PrintBodyData(x);
            }
        }

    }
}
