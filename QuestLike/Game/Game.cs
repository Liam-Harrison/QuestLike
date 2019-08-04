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

        public static bool SaveExistWithName(string savename)
        {
            return File.Exists(GetSavePath(savename));
        }
        
        public static string[] GetAllSaveNames()
        {
            List<string> saves = new List<string>();
            foreach (var file in Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}/saves/"))
            {
                if (TryParseSaveFile(file, out SaveData save))
                {
                    saves.Add(file);
                }
            }
            return saves.ToArray();
        }

        public static bool TryParseSaveFile(string path, out SaveData save)
        {
            save = null;
            if (!File.Exists(path)) return false;
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    //TypeNameHandling = TypeNameHandling.All, <- breaks it?
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                };

                var json = File.ReadAllText(path);
                save = JsonConvert.DeserializeObject<SaveData>(json, settings);
                return true;
            }
            catch(Exception e)
            {
                GameScreen.PrintLine($"\n{e.Message}\n");
                return false;
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class SaveData
        {
            public Player player;
            public Room[] rooms;
            [JsonProperty(IsReference = true)]
            public int roomIndex;
            public CircleBuffer<string> buffer;
        }

        public static void LoadGame(string savename)
        {
            if (TryParseSaveFile(GetSavePath(savename), out SaveData save)) {
                GameScreen.buffer = save.buffer;
                rooms = new List<Room>(save.rooms);
                roomIndex = save.roomIndex;
                player = save.player;
                ChangeRoom(rooms[roomIndex]);
                GameScreen.LoadBuffer();
                GameScreen.PrintLine("Loaded save!");
            }
            else if (Settings.DebugMode)
            {
                GameScreen.PrintLine("Could not load savefile.");
            }
        }

        public static string GetSavePath(string savename)
        {
            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/saves/")) Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}/saves/");
            return $"{AppDomain.CurrentDomain.BaseDirectory}/saves/{savename}.save";
        }

        public static void SaveGame(string savename)
        {
            var save = new SaveData()
            {
                buffer = GameScreen.buffer,
                rooms = rooms.ToArray(),
                player = GetPlayer,
                roomIndex = roomIndex
            };
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                };

                var json = JsonConvert.SerializeObject(save, settings);
                File.WriteAllText(GetSavePath(savename), json);
            }
            catch (Exception e)
            {
                GameScreen.PrintLine($"Could not save file.\n\n{e.Message}\n");
            }
        }

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

        public static void ChangeRoom(Room room)
        {
            roomIndex = rooms.IndexOf(room);
            if (GameScreen.roomconsole != null)
            {
                GameScreen.roomconsole.PrintCentre(GameScreen.roomconsole.Width / 2, 0, GetRoom.Name, new Cell(Color.Black, Color.LightGray));
            }
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
            var found = GetPlayer.LocateWithGameID(gameID);
            if (found != null) return found;
            found = GetRoom.LocateWithGameID(gameID);
            if (found != null) return found;
            return null;
        }
    }
}
