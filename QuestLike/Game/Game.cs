using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Command;

namespace QuestLike
{
    static class Game
    {
        private static List<Room> rooms = new List<Room>();
        private static Room currentRoom;
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
                return currentRoom;
            }
        }

        public static void Update()
        {
            player.Update();
            foreach (var room in rooms) room.Update();
            GameScreen.miniConsole.DrawMiniScreen();
        }

        public static void EndTurn()
        {
            Update();
        }

        public static void AddRoom(Room room)
        {
            rooms.Add(room);
        }

        public static void ChangeRoom(Room room)
        {
            currentRoom = room;
        }

        public static IHoldable[] LocateHoldables(string id, bool firstcall = true)
        {
            List<IHoldable> holdables = new List<IHoldable>();
            var found = Locate(id, firstcall);
            foreach (var item in found)
            {
                if (item is IHoldable) holdables.Add(item as IHoldable);
            }
            return holdables.ToArray();
        }

        public static IInventory[] LocateInventories(string id, bool firstcall = true)
        {
            List<IInventory> inventories = new List<IInventory>();
            var found = Locate(id, false);
            foreach (var item in found)
            {
                if (item is IInventory) inventories.Add(item as IInventory);
            }
            return inventories.ToArray();
        }

        public static bool ProcessInput(string input, bool admin = false)
        {
            if (CommandFactory.CommandMatches(input)) return CommandFactory.GetMatchingCommand(input).Execute(admin);
            return false;
        }

        public static T[] LocateObjectsWithType<T>(bool firstcall = true) where T : class
        {
            List<T> results = new List<T>();
            results.AddRange(player.LocateObjectsWithType<T>(firstcall));
            results.AddRange(currentRoom.LocateObjectsWithType<T>(firstcall));

            return results.ToArray();
        }

        public static T[] LocateObjectsWithType<T>(string id, bool firstcall = true) where T : class
        {
            List<T> results = new List<T>();
            results.AddRange(player.LocateObjectsWithType<T>(id, firstcall));
            results.AddRange(currentRoom.LocateObjectsWithType<T>(id, firstcall));

            return results.ToArray();
        }

        public static GameObject[] Locate(string id, bool firstcall = true)
        {
            List<GameObject> results = new List<GameObject>();
            results.AddRange(player.Locate(id, firstcall));
            results.AddRange(currentRoom.Locate(id, firstcall));

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

        public static void LocateSingleHoldableObject(string id, PromptObjectResponse<IHoldable> response, bool firstcall = true)
        {
            var holdables = LocateHoldables(id, firstcall);

            if (holdables.Length == 0) response.Invoke(null, false);
            else if (holdables.Length == 1) response.Invoke(holdables[0] as IHoldable, false);
            else Utilities.PromptSelection<IHoldable>("Select one of the following with the matching tag \"" + id + "\"", Utilities.CastArray<GameObject, IHoldable>(holdables), response);
        }

        public static void LocateSingleInventory(string id, PromptObjectResponse<IInventory> response, bool firstcall = true)
        {
            var holdables = LocateInventories(id, false);

            if (holdables.Length == 0) response.Invoke(null, false);
            else if (holdables.Length == 1) response.Invoke(holdables[0], false);
            else Utilities.PromptSelection<IInventory>("Select one of the following with the matching tag \"" + id + "\"", Utilities.CastArray<GameObject, IInventory>(holdables), response);
        }

        public static GameObject LocateWithGameID(int gameID)
        {
            var found = player.LocateWithGameID(gameID);
            if (found != null) return found;
            found = currentRoom.LocateWithGameID(gameID);
            if (found != null) return found;
            return null;
        }
    }
}
