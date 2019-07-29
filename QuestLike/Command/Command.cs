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

namespace QuestLike.Command
{
    abstract class Command
    {
        protected List<string> args = new List<string>();
        protected string[] keywords;
        protected string[] usecases;
        protected int[] adminCommands;
        public string[] tags;
        public string commandName;
        protected int usecaseID = -1;
        protected string title = "";
        protected string description = "";
        public bool showInHelp = true;

        public Command()
        {

        }

        public string GetArg(int index)
        {
            return args[index];
        }

        public string HelpString
        {
            get
            {
                string text = "\n" + commandName + " Command.";

                for (int i = 0; i < usecases.Count(); i++)
                {
                    var usecase = usecases[i];
                    if (adminCommands != null && adminCommands.Contains(i)) continue;

                    var splitted = usecase.Split(' ');
                    string command = "\n";
                    foreach (var word in splitted)
                    {
                        if (word == "^" || word == "$")
                        {
                            command += $" <{Color.Orange.ToInteger()},>___@";
                        }
                        else if (word == "+")
                        {
                            command += $" <{Color.Orange.ToInteger()},>[number]@";
                        }
                        else if (word == "_")
                        {
                            command += $" <{Color.Orange.ToInteger()},>[";
                            foreach (var item in keywords)
                            {
                                command += item;
                                if (keywords.Last() == item) command += "]@";
                                else command += ", ";
                            }
                        }
                        else
                        {
                            command += " " + word;
                        }
                    }
                    text += command;
                }

                return text;
            }
        }

        /// <summary>
        /// The method called to execute a command.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <returns>True if turn ends - otherwise returns false.</returns>
        public abstract bool Execute(bool admin);

        public bool Matches(string input)
        {
            string[] input_array = input.Split(' ');
            args.Clear();
            int a = 0;

            bool readingLong = false;
            int readingLongArgIndex = -1;
            foreach (string sentence in usecases)
            {

                string[] expected_array = sentence.Split(' ');

                if (input_array.Length < expected_array.Length)
                {
                    a++;
                    continue;
                }

                int expected_index = 0;
                for (int i = 0; i < input_array.Length; i++)
                {
                    if ((expected_index >= expected_array.Length) && !readingLong) break;

                    if (expected_array[expected_index] == "_" && keywords.Contains(input_array[i]))
                    {
                        if (readingLong) readingLong = false;

                        if (i == input_array.Length - 1 && expected_index == expected_array.Length - 1)
                        {
                            usecaseID = a;
                            return true;
                        }
                        expected_index++;
                        continue;
                    }
                    else if (expected_array[expected_index] == input_array[i])
                    {
                        if (readingLong) readingLong = false;

                        if (i == input_array.Length - 1 && expected_index == expected_array.Length - 1)
                        {
                            usecaseID = a;
                            return true;
                        }
                        expected_index++;
                        continue;
                    }
                    else if (expected_array[expected_index] == "$")
                    {
                        if (readingLong) readingLong = false;

                        args.Add(input_array[i]);

                        if (i == input_array.Length - 1 && expected_index == expected_array.Length - 1)
                        {
                            usecaseID = a;
                            return true;
                        }
                        expected_index++;
                        continue;
                    }
                    else if (expected_array[expected_index] == "^")
                    {
                        if (readingLong)
                        {
                            args[readingLongArgIndex] += " " + input_array[i];
                        }
                        else
                        {
                            readingLong = true;
                            readingLongArgIndex = args.Count;
                            args.Add(input_array[i]);
                        }

                        if (i == input_array.Length - 1 && expected_index == expected_array.Length - 1)
                        {
                            usecaseID = a;
                            return true;
                        }
                        if (expected_index + 1 < expected_array.Length) expected_index++;
                        continue;
                    }
                    else if (expected_array[expected_index] == "+" && int.TryParse(input_array[i], out int result))
                    {
                        args.Add(input_array[i]);

                        if (i == input_array.Length - 1 && expected_index == expected_array.Length - 1)
                        {
                            usecaseID = a;
                            return true;
                        }
                        if (expected_index + 1 < expected_array.Length) expected_index++;
                        continue;
                    }
                    else if (readingLong)
                    {
                        args[readingLongArgIndex] += " " + input_array[i];

                        if (i == input_array.Length - 1 && expected_index == expected_array.Length - 1)
                        {
                            usecaseID = a;
                            return true;
                        }
                        continue;
                    }
                    break;
                }

                args.Clear();
                a++;
            }
            return false;
        }
    }
}
