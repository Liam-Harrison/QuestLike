using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using QuestLike.Command;
using SadConsole;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

namespace QuestLike
{
    static class Game
    {
        private static List<Room> rooms = new List<Room>();
        private static int roomIndex;
        private static Player player = new Player();

        public static int Padding
        {
            get
            {
                return 16;
            }
        }

        public static string Pad(this string content)
        {
            return content.PadRight(Padding, '.') + ": ";
        }

        public static string Pad(this string content, int length)
        {
            return content.PadRight(length, '.') + ": ";
        }

        public static string Pad(this string content, char character)
        {
            return content.PadRight(Padding, character);
        }

        public static string Pad(this string content, char character, int length)
        {
            return content.PadRight(length, character);
        }

        public static Player GetPlayer
        {
            get
            {
                return player;
            }
        }

        public static Room GetRoom
        {
            get
            {
                return rooms[roomIndex];
            }
        }

        public static void Update()
        {
            player.Update();
            foreach (var room in rooms) room.Update();
        }

        public static void AddRoom(Room room)
        {
            rooms.Add(room);
        }

        public static void AddSetRoom(Room room)
        {
            rooms.Add(room);
            ChangeRoom(room);
        }

        public static void ChangeRoom(Room room)
        {
            roomIndex = rooms.IndexOf(room);
            if (GameScreen.roomconsole != null)
            {
                GameScreen.roomconsole.PrintCentre(GameScreen.roomconsole.Width / 2, 0, GetRoom.Name, new Cell(Color.Black, Color.LightGray));
            }
        }

        public static bool ProcessInput(string input, bool admin = false)
        {
            if (CommandFactory.CommandMatches(input)) return CommandFactory.GetMatchingCommand(input).Execute(admin);
            return false;
        }

        public static T[] LocateObjectsWithType<T>(bool firstcall = true) where T : class
        {
            List<T> results = new List<T>();
            results.AddRange(GetPlayer.LocateObjectsWithType<T>(firstcall));
            results.AddRange(GetRoom.LocateObjectsWithType<T>(firstcall));

            return results.ToArray();
        }

        public static T[] LocateObjectsWithType<T>(string id, bool firstcall = true) where T : class
        {
            List<T> results = new List<T>();
            results.AddRange(GetPlayer.LocateObjectsWithType<T>(id, firstcall));
            results.AddRange(GetRoom.LocateObjectsWithType<T>(id, firstcall));

            return results.ToArray();
        }

        public static GameObject[] Locate(string id, bool firstcall = true)
        {
            List<GameObject> results = new List<GameObject>();
            results.AddRange(GetPlayer.Locate(id, firstcall));
            results.AddRange(GetRoom.Locate(id, firstcall));

            return results.ToArray();
        }

        public static void LocateSingleObject(string id, PromptObjectResponse<GameObject> response, bool firstcall = true)
        {
            GameObject[] results = Locate(id, firstcall);

            if (results.Length == 0) response.Invoke(null, false);
            else if (results.Length == 1) response.Invoke(results[0], false);
            else Utilities.PromptSelection("Select one of the following with the matching tag \"" + id + "\"", results, response);
        }

        public static void LocateSingleObjectOfType<T>(PromptObjectResponse<T> response, bool firstcall = true) where T : class
        {
            var results = Utilities.CastArray<GameObject, T>(LocateObjectsWithType<T>(firstcall));

            if (results.Length == 0) response.Invoke(null, false);
            else if (results.Length == 1) response.Invoke(results[0] as T, false);
            else Utilities.PromptSelection<T>("Select one of the following from these options", results, response);
        }

        public static void LocateSingleObjectOfType<T>(string id, PromptObjectResponse<T> response, bool firstcall = true) where T : class
        {
            var results = Utilities.CastArray<GameObject, T>(LocateObjectsWithType<T>(id, firstcall));

            if (results.Length == 0) response.Invoke(default(T), false);
            else if (results.Length == 1) response.Invoke(results[0] as T, false);
            else Utilities.PromptSelection<T>("Select one of the following with the matching tag \"" + id + "\"", results, response);
        }

        public static GameObject LocateWithGameID(int gameID)
        {
            var found = GetPlayer.LocateWithGameID(gameID);
            if (found != null) return found;
            found = GetRoom.LocateWithGameID(gameID);
            if (found != null) return found;
            return null;
        }
    }
}
