using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestLike
{
    
    class Wall : GameObject
    {
        public Wall(): base("", new string[] { })
        {
            blocking = true;
        }

        public Wall(string name, string[] ids) : base(name, ids)
        {
            blocking = true;
        }

        public Wall(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            blocking = true;
        }

        public Wall(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            blocking = true;
        }

        protected virtual bool IsWallInDirection(Direction direction)
        {
            var objects = GameScreen.miniConsole.GetAllObjectsAtPoint(Pathfinding.DirectionToPoint(direction) + Position);
            foreach (var item in objects)
            {
                if (item is Wall && !(item is RangeWall)) return true;
            }
            return false;
        }

        
        public override string ScreenChar
        {
            get
            {
                var northwall = IsWallInDirection(Direction.North);
                var eastwall = IsWallInDirection(Direction.East);
                var southwall = IsWallInDirection(Direction.South);
                var westwall = IsWallInDirection(Direction.West);

                if (northwall && eastwall && southwall && westwall)
                {
                    return ((char)206).ToString();
                }
                else if (northwall && eastwall && westwall)
                {
                    return ((char)202).ToString();
                }
                else if (southwall && eastwall && westwall)
                {
                    return ((char)203).ToString();
                }
                else if (northwall && eastwall && southwall)
                {
                    return ((char)204).ToString();
                }
                else if (northwall && westwall && southwall)
                {
                    return ((char)185).ToString();
                }
                else if (northwall && eastwall)
                {
                    return ((char)200).ToString();
                }
                else if (northwall && westwall)
                {
                    return ((char)188).ToString();
                }
                else if (eastwall && southwall)
                {
                    return ((char)201).ToString();
                }
                else if (westwall && southwall)
                {
                    return ((char)187).ToString();
                }
                else if (northwall || southwall)
                {
                    return ((char)186).ToString();
                }
                else if (westwall || eastwall)
                {
                    return ((char)205).ToString();
                }
                return ((char)205).ToString();
            }
        }
    }
}
