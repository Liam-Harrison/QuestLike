using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using QuestLike;
using QuestLike.Combat;
using Microsoft.Xna.Framework;

namespace QuestLike
{
    class Entity : GameObject, IInventory
    {
        private Inventory inventory;
        public int moveYAxis = 2;
        public int moveXAxis = 4;

        public Entity(string name, string[] ids) : this(name, "", ids)
        {
        }

        public Entity(string name, string desc, string[] ids) : this(name, "", desc, ids)
        {
        }

        public Entity(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            inventory = new Inventory(this);
            AddCollection<BodyPart>();
            GetCollection<BodyPart>().treeSearchable = false;
            AddCollection<Item>();
        }

        private bool moverun = false;
        private Point startpoint;

        public Point TurnStartPoint
        {
            get
            {
                if (!moverun) return Position;
                return startpoint;
            }
        }

        public Pathfinding.BFSPoint[] MoveArea
        {
            get
            {
                return Pathfinding.SpillGetPoints(TurnStartPoint, moveXAxis, moveYAxis, true, true);
            }
        }

        public int Distance(Point start, Point end)
        {
            int x = Math.Abs(end.X - start.X);
            int y = Math.Abs(end.Y - start.Y);
            return Math.Abs(x) + Math.Abs(y);
        }

        public int LargestDistance(Point start, Point end)
        {
            int x = Math.Abs(end.X - start.X);
            int y = Math.Abs(end.Y - start.Y);
            if (x > y) return x;
            else return y;
        }

        public int DistanceXAxis(Point start, Point end)
        {
            return Math.Abs(end.X - start.X);
        }

        public int DistanceYAxis(Point start, Point end)
        {
            return Math.Abs(end.Y - start.Y);
        }

        public override void Move(Direction direction)
        {
            var move = Pathfinding.DirectionToPoint(direction);
            var newposition = Position + move;
            if (Pathfinding.IsPointInBFSList(newposition, MoveArea))
            {
                if (!moverun)
                {
                    moverun = true;
                    startpoint = position;
                }

                position = newposition;
            }
        }

        public override void Update()
        {
            moverun = false;

            base.Update();

            foreach (var i in GetCollection<BodyPart>().GetAllObjects())
            {
                i.Update();
            }
        }

        public override string Examine
        {
            get
            {
                string text = base.Examine;

                return text;
            }
        }

        public Inventory GetInventory => inventory.GetInventory;
    }
}
