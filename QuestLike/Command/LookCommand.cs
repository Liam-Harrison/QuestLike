using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Organs;
using SadConsole;
using Console = SadConsole.Console;
using Global = SadConsole.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Input;
using ZorkLike;

namespace ZorkLike.Command
{
    class LookCommand : Command
    {

        public LookCommand()
        {
            keywords = new string[] { "look", "examine" };
            usecases = new string[] { "look", "look at my items", "look at my ^", "look at +", "look at ^ on my ^", "look at ^ on ^", "look at ^", "_ my ^"};
            adminCommands = new int[] { 3 };
            tags = new string[] { "look" };
            commandName = "Look";
        }

        public override bool Execute(bool admin)
        {
            switch (usecaseID)
            {
                case 0:

                    GameScreen.NewLine();
                    GameScreen.PrintLine(Game.GetRoom.Examine);

                    break;
                case 1:

                    var gameobjects = new List<GameObject>();
                    var holdables = ZorkLike.Game.GetPlayer.LocateObjectsWithType<IHoldable>(false);
                    var inventories = ZorkLike.Game.GetPlayer.LocateObjectsWithType<IInventory>(false);

                    foreach (var holdable in holdables)
                    {
                        if (holdable.GetHoldingSafe(out Item item)) gameobjects.Add(item);
                    }

                    foreach (var inventory in inventories)
                    {
                        foreach (var item in inventory.GetInventory.GetItems)
                        {
                            gameobjects.Add(item);
                        }
                    }

                    if (gameobjects.Count == 0)
                    {
                        GameScreen.NewLine();
                        GameScreen.PrintLine("You don't have any items on you right now.");
                        break;
                    }
                    GameScreen.NewLine();
                    GameScreen.PrintLine("You have these items on you:");
                    int longestname = 0, longestcollectionname = 0;
                    foreach (var item in gameobjects)
                    {
                        if (item.Name.Count() > longestname) longestname = item.Name.Count();
                        if (item.container.owner != null && item.container.owner.Name.Count() > longestcollectionname)
                            longestcollectionname = item.container.owner.Name.Count();
                    }
                    foreach (var item in gameobjects)
                    {
                        GameScreen.PrintLine($" - <{Color.Cyan.ToInteger()},look at {item.ID}>" + (item.Name.PadRight(longestname + 2, ' ')) + "@");
                    }
                    break;
                case 2:

                    Game.GetPlayer.LocateSingleObject(args[0], (i, b) =>
                    {
                        if (i == null)
                        {
                            GameScreen.NewLine();
                            GameScreen.PrintLine("Could not find anything like that.");
                            return;
                        }

                        GameScreen.NewLine();
                        GameScreen.PrintLine(i.Examine);
                    }, false);

                    break;
                case 3:

                    if (!admin) break;

                    if (int.TryParse(GetArg(0), out int gameid))
                    {
                        var found = Game.LocateWithGameID(gameid);
                        if (found == null)
                        {
                            if (Settings.DebugMode)
                            {
                                GameScreen.PrintLine($"\n<{Color.Red.ToInteger()},>Tried to administratively find a gameobject by ID and failed.@");
                            }
                            break;
                        }

                        GameScreen.NewLine();
                        GameScreen.PrintLine(found.Examine);
                    }

                    break;
                case 4:


                    break;
                case 5:

                    Game.LocateSingleObject(args[1], (j, b) => 
                    {
                        if (j == null)
                        {
                            GameScreen.NewLine();
                            GameScreen.PrintLine("Could not find anything like that.\n");
                            return;
                        }

                        j.LocateSingleObject(args[0], (i, c) =>
                        {
                            if (i == null)
                            {
                                GameScreen.NewLine();
                                GameScreen.PrintLine("Could not find anything like that.");
                                return;
                            }

                            GameScreen.NewLine();
                            GameScreen.PrintLine(i.Examine);
                        }, false);
                    });

                    break;
                case 6:

                    Game.LocateSingleObject(args[0], (i,b) =>
                    {
                        if (i == null)
                        {
                            GameScreen.NewLine();
                            GameScreen.PrintLine("Could not find anything like that.");
                            return;
                        }

                        GameScreen.NewLine();
                        GameScreen.PrintLine(i.Examine);
                    });

                    break;
                case 7:

                    Game.GetPlayer.LocateSingleObject(args[0], (i, b) =>
                    {
                        if (i == null)
                        {
                            GameScreen.NewLine();
                            GameScreen.PrintLine("Could not find anything like that.");
                            return;
                        }

                        GameScreen.NewLine();
                        GameScreen.PrintLine(i.Examine);
                    });

                    break;
            }

            return false;
        }

    }
}
