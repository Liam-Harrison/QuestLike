﻿using System;
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

namespace QuestLike.Command
{
    class UseCommand : Command
    {
        public UseCommand()
        {
            keywords = new string[] { "use", "activate" };
            usecases = new string[] { "use +", "use + ^ on ^", "_ ^ on ^", "_ ^" };
            tags = new string[] { "use", "activate" };
            adminCommands = new int[] { 0 };
            commandName = "Use";
        }

        public override bool Execute(bool admin)
        {
            switch (usecaseID)
            {
                case 0:
                    if (!admin) return false;
                    if (int.TryParse(GetArg(0), out int id))
                    {
                        var gameobject = Game.LocateWithGameID(id);
                        if (gameobject is IUseable) (gameobject as IUseable).Use(null, null);
                    }
                    break;
                case 1:
                    GameScreen.PrintLine("\nI'm afraid using items on entities hasn't been implemented yet.");
                    break;
                case 2:
                    GameScreen.PrintLine("\nI'm afraid using items on entities hasn't been implemented yet.");
                    break;
                case 3:
                    Game.GetPlayer.LocateSingleObject(GetArg(0), (item, b) =>
                    {
                        if (item == null)
                        {
                            GameScreen.PrintLine("\nCould not find an item by that description.\n");
                            return;
                        }
                        GameScreen.PrintLine($"\nAttempting to use <{Color.Cyan.ToInteger()},>" + item.Name + "@\n");
                    }, false);

                    break;
            }
            return false;
        }
    }
}
