using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike
{
    class Room : GameObject
    {
        public Room() : base("Room", new string[] { "room", "space" })
        {
            locator.FreezeSearchesBelow = false;
            AddCollection<Entity>();
            AddCollection<GameObject>();
            AddCollection<Wall>();
            GetCollection<Wall>().showInLocate = false;
        }

        public Room(string name) : base(name, new string[] { "room", "space" })
        {
            locator.FreezeSearchesBelow = false;
            AddCollection<Entity>();
            AddCollection<GameObject>();
            AddCollection<Wall>();
            GetCollection<Wall>().showInLocate = false;
        }

        public Room(string[] ids) : base("Room", ids)
        {
            locator.FreezeSearchesBelow = false;
            AddCollection<Entity>();
            AddCollection<GameObject>();
            AddCollection<Wall>();
            GetCollection<Wall>().showInLocate = false;
        }

        public Room(string name, string[] ids) : base(name, ids)
        {
            locator.FreezeSearchesBelow = false;
            AddCollection<Entity>();
            AddCollection<GameObject>();
            AddCollection<Wall>();
            GetCollection<Wall>().showInLocate = false;
        }

        public void SetRoom(char[][] map)
        {
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '#') GetCollection<Wall>().AddObject(new Wall() { position = new Microsoft.Xna.Framework.Point(x, y)});
                }
            }
        }

        public override void Update()
        {
            base.Update();

            foreach (var gameobject in GetCollection<GameObject>().Objects) gameobject.Update();
        }

        public bool IsObjectInRoomDirectly(GameObject gameObject)
        {
            foreach (var item in GetCollection<GameObject>().Objects)
            {
                if (item == gameObject) return true;
            }
            return false;
        }

        public void PrefillEdgesWithWalls()
        {
            for (int y = 0; y <= 10; y++)
            {
                GetCollection<Wall>().AddObject(new Wall() { position = new Microsoft.Xna.Framework.Point(0, y) });
                GetCollection<Wall>().AddObject(new Wall() { position = new Microsoft.Xna.Framework.Point(23, y) });
            }

            for (int x = 0; x <= 23; x++)
            {
                GetCollection<Wall>().AddObject(new Wall() { position = new Microsoft.Xna.Framework.Point(x, 0) });
                GetCollection<Wall>().AddObject(new Wall() { position = new Microsoft.Xna.Framework.Point(x, 10) });
            }
        }
    }
}
