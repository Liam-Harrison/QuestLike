using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Command
{
    class HelpCommand : Command
    {
        public HelpCommand()
        {
            keywords = new string[] { };
            usecases = new string[] { "help", "help ^" };
            tags = new string[] { "help" };
            commandName = "Help";
        }

        public override bool Execute(bool admin)
        {
            switch (usecaseID)
            {
                case 0:

                    GameScreen.PrintLine($"\nHeres a couple quick tips:");

                    foreach (var command in CommandFactory.Commands)
                    {
                        if (command.showInHelp)
                        {
                            GameScreen.PrintLine(command.HelpString);
                        }
                    }

                    break;
                case 1:

                    var found = CommandFactory.GetCommandWithTag(args[0]);

                    if (found == null || !found.showInHelp)
                    {
                        GameScreen.PrintLine("\nCould not find a command with the name \"" + args[0] + "\"");
                        return false;
                    }

                    GameScreen.PrintLine(found.HelpString);

                    break;
            }
            return false;
        }
    }
}
