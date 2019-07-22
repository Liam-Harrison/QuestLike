using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Command
{
    static class CommandFactory
    {
        private static Command[] commands = new Command[]
        {
            new LookCommand(),
            new DebugCommand(),
            new HelpCommand(),
            new GrabCommand(),
            new UseCommand(),
            new InteractionCommands()
        };

        public static Command GetCommandWithTag(string tag)
        {
            foreach (var command in commands)
            {
                foreach (var ctag in command.tags)
                {
                    if (tag == ctag) return command;
                }
            }
            return null;
        }

        public static Command[] Commands
        {
            get
            {
                return commands;
            }
        }

        public static Command GetMatchingCommand(string input)
        {
            foreach (var command in commands)
            {
                if (command.Matches(input))
                {
                    return command;
                }
            }
            return null;
        }

        public static bool CommandMatches(string input)
        {
            return GetMatchingCommand(input) != null;
        }
    }
}
