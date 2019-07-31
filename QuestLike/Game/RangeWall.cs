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
            var objects = GameScreen.miniConsole.GetAllObjectsAtPoint(Pathfinding.DirectionToPoint(direction) + Position);
            foreach (var item in objects)
            {
                if (item is RangeWall) return true;
            }
            return false;
        }

    }
}
