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
using Game = QuestLike.Game;

namespace QuestLike
{
    class RangeWall : Wall
    {
        public RangeWall() : base("", new string[] { })
        {
            screenCell = new Cell(Color.Orange);
        }

        public RangeWall(string name, string[] ids) : base(name, ids)
        {
            screenCell = new Cell(Color.Orange);
        }

        public RangeWall(string name, string desc, string[] ids) : base(name, desc, ids)
        {
            screenCell = new Cell(Color.Orange);
        }

        public RangeWall(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            screenCell = new Cell(Color.Orange);
        }

        protected override bool IsWallInDirection(Direction direction)
        {
            return GameScreen.miniConsole.IsRangeWallAtPoint(Pathfinding.DirectionToPoint(direction) + Position);
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
                    return ((char)197).ToString();
                }
                else if (northwall && eastwall && westwall)
                {
                    return ((char)193).ToString();
                }
                else if (southwall && eastwall && westwall)
                {
                    return ((char)194).ToString();
                }
                else if (northwall && eastwall && southwall)
                {
                    return ((char)195).ToString();
                }
                else if (northwall && westwall && southwall)
                {
                    return ((char)180).ToString();
                }
                else if (northwall && eastwall)
                {
                    return ((char)192).ToString();
                }
                else if (northwall && westwall)
                {
                    return ((char)217).ToString();
                }
                else if (eastwall && southwall)
                {
                    return ((char)218).ToString();
                }
                else if (westwall && southwall)
                {
                    return ((char)191).ToString();
                }
                else if (northwall || southwall)
                {
                    return ((char)179).ToString();
                }
                else if (westwall || eastwall)
                {
                    return ((char)196).ToString();
                }
                return ((char)196).ToString();
            }
        }
    }
}
