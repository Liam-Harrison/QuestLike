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

namespace QuestLike
{
    public delegate void PromptObjectResponse<T>(T selected, bool cancelled) where T : class;
    public delegate void PromptYesNoResponse(bool answer, bool cancelled);

    static class Utilities
    {

        public static float Clamp(float a, float min, float max)
        {
            if (a > max) return max;
            else if (a < min) return min;
            else return a;
        }

        public static int Clamp(int a, int min, int max)
        {
            if (a > max) return max;
            else if (a < min) return min;
            else return a;
        }

        public static float GetBetweenValues(float a, float b, float percentage)
        {
            return a + (b - a) * percentage;
        }

        public enum UserResponseType
        {
            None,
            GameObject,
            Index,
            YesNo
        }

        public static UserResponseType awaitedUserResponse = UserResponseType.None;
        public static dynamic userResponse = null;
        public static object[] selectableObjects = null;
        public static Type responseType;
        public static string uuid;

        public static void PromptSelection<T>(string showString, object[] list, PromptObjectResponse<T> response) where T: class
        {
            responseType = typeof(T);
            userResponse = response;
            selectableObjects = list;
            awaitedUserResponse = UserResponseType.GameObject;
            var listed = new List<object>();
            listed.AddRange(list);
            var max = listed.Max((a) => { if (a is GameObject) { return (a as GameObject).Name.Length; } else return a.ToString().Length; });
            var maxContainerName = listed.Max((a) => 
            {
                if (a is GameObject)
                {
                    if ((a as GameObject).container == null || (a as GameObject).container.Owner is Room) return 0;
                    return (a as GameObject).container.Owner.Name.Length;
                }
                return 0;
            });

            if (list.Length == 1)
            {
                response.Invoke(list[0] as T, false);
                return;
            }
            else if (list.Length > 1)
            {
                GameScreen.NewLine();
                GameScreen.PrintLine(showString);
                uuid = System.Guid.NewGuid().ToString();
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] is GameObject)
                    {
                        awaitedUserResponse = UserResponseType.GameObject;
                        GameObject j = list[i] as GameObject;
                        GameScreen.PrintLine($"[" + (i + 1) + $"] - <{Color.Cyan.ToInteger()},select " + j.ID + " uuid " + uuid + ">" + (j.Name.PadRight(max, ' ')) + "@");
                        if (j.container != null && !(j.container.Owner is Room)) GameScreen.Print(" - Attatched to " + (j.container.Owner.Name.PadRight(maxContainerName, ' ')));
                        if (j.ShortDescription != "") GameScreen.Print(" - " + j.ShortDescription);
                    }
                    else
                    {
                        awaitedUserResponse = UserResponseType.Index;
                        string name = list[i].ToString();
                        GameScreen.PrintLine($"[" + (i + 1) + $"] - <{Color.Cyan.ToInteger()},index select " + i + " uuid " + uuid + ">" + (name.PadRight(max, ' ')) + "@");
                    }
                }
            }
        }

        public static List<T> AsList<T>(this T[] objects)
        {
            return new List<T>(objects);
        }

        public static T[] CastArray<T, T2>(T2[] input) where T: class
        {
            List<T> toReturn = new List<T>();
            foreach (var item in input)
            {
                if (item is T) toReturn.Add(item as T);
            }
            return toReturn.ToArray();
        }

        public static void PromptYesNo(PromptYesNoResponse response)
        {
            awaitedUserResponse = UserResponseType.YesNo;
            userResponse = response;
            GameScreen.PrintLine($"[<{Color.LightGreen.ToInteger()},answer yes>Yes@]  [<{Color.OrangeRed.ToInteger()},answer no>No@]");
        }

    }
}
