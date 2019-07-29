using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using Microsoft.Xna.Framework;

namespace QuestLike.Command
{
    class EquipCommand : Command
    {
        public EquipCommand()
        {
            keywords = new string[] { "equip" };
            usecases = new string[] { "equip +", "equip ^ on ^", "equip ^" };
            tags = new string[] { "equip" };
            adminCommands = new int[] { 0 };
            commandName = "Equip";
        }

        public override bool Execute(bool admin)
        {

            switch(usecaseID)
            {
                case 0:
                    if (int.TryParse(GetArg(0), out int id))
                    {
                        var gameobject = Game.LocateWithGameID(id);
                        if (gameobject == null) return false;
                        if (!(gameobject is Equipable)) return false;

                        var points = new List<GameObject>();
                        foreach (var part in Game.GetPlayer.LocateObjectsWithType<BodyPart>(false))
                        {
                            if (part.CanEquipItem(gameobject as Equipable))
                            {
                                if (part.EquipedItem == gameobject) continue;
                                points.Add(part);
                            }
                        }

                        if (points.Count == 0)
                        {
                            GameScreen.PrintLine("\nYou have nowhere to equip that item.");
                            return false;
                        }

                        Utilities.PromptSelection<GameObject>("Where do you want to equip this item?", points.ToArray(), (part, canceled) => 
                        {
                            if (canceled) return;
                            var casted = part as BodyPart;
                            if (!casted.HasItemEquiped)
                            {
                                casted.EquipItem(gameobject as Equipable);
                                GameScreen.PrintLine($"\nEquiped <{Color.Cyan.ToInteger()},look at {gameobject.ID}>{gameobject.Name}@ to <{Color.Cyan.ToInteger()},look at {casted.ID}>{casted.Name}@");
                                return;
                            }
                        });
                    }
                    break;
            }

            return false;
        }
    }
}
