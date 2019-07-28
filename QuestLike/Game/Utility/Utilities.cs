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
using ZorkLike;

namespace ZorkLike
{
    public delegate void PromptObjectResponse<T>(T selected, bool cancelled) where T : class;
    public delegate void PromptYesNoResponse(bool answer, bool cancelled);

    class Utilities
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

        public static void SaveScreenBuffer()
        {

        }

        public enum UserResponseType
        {
            None,
            GameObject,
            YesNo
        }

        public static UserResponseType awaitedUserResponse = UserResponseType.None;
        public static dynamic userResponse = null;
        public static GameObject[] selectableObjects = null;
        public static Type responseType;

        public static void PromptSelection<T>(string showString, GameObject[] list, PromptObjectResponse<T> response) where T: class
        {
            responseType = typeof(T);
            userResponse = response;
            selectableObjects = list;
            awaitedUserResponse = UserResponseType.GameObject;
            var listed = new List<GameObject>();
            listed.AddRange(list);
            var max = listed.Max((a) => { return a.Name.Length; });
            var maxContainerName = listed.Max((a) => 
            {
                if (a.container == null || a.container.owner is Room) return 0;
                return a.container.owner.Name.Length;
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
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] is GameObject)
                    {
                        GameObject j = list[i] as GameObject;
                        GameScreen.PrintLine($"[" + (i + 1) + $"] - <{Color.Cyan.ToInteger()},select " + j.ID + ">" + (j.Name.PadRight(max, ' ')) + "@");
                        if (j.container != null && !(j.container.owner is Room)) GameScreen.Print(" - Attatched to " + (j.container.owner.Name.PadRight(maxContainerName, ' ')));
                        if (j.ShortDescription != "") GameScreen.Print(" - " + j.ShortDescription);
                    }
                }
                GameScreen.NewLine();
            }
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
