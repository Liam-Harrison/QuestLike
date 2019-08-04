using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using QuestLike;
using QuestLike.Combat;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace QuestLike
{
    public struct Range
    {
        public Range(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }

    public struct InteractionRanges
    {
        public Range grabRange;
        public Range meleeRange;
        public Range rangeRange;
        public Range spellRange;
        public Range delicateRange;
        public Range useRange;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class Entity : GameObject, IInventory
    {
        [JsonRequired]
        private Inventory inventory;
        [JsonRequired]
        public int moveYAxis = 2;
        [JsonRequired]
        public int moveXAxis = 4;
        [JsonIgnore]
        public Action<ITalkable> talkaction;
        [JsonIgnore]
        public Action<INPC> onupdate;

        [JsonRequired]
        InteractionRanges interactionRanges = new InteractionRanges() {
            delicateRange = new Range(2, 1),
            grabRange = new Range(4, 2),
            meleeRange = new Range(2, 1),
            rangeRange = new Range(6, 3),
            spellRange = new Range(4, 2),
            useRange = new Range(2, 1)
        };

        public Entity() : base()
        {

        }

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
            AddCollection<Item>();
        }

        [JsonRequired]
        private bool moverun = false;
        [JsonRequired]
        private Point startpoint;

        [JsonIgnore]
        public Point TurnStartPoint
        {
            get
            {
                if (!moverun) return Position;
                return startpoint;
            }
        }

        [JsonIgnore]
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

        [JsonIgnore]
        public override string Examine
        {
            get
            {
                string text = base.Examine;

                return text;
            }
        }

        [JsonIgnore]
        public Inventory GetInventory => inventory;

        [JsonIgnore]
        internal InteractionRanges InteractionRanges { get => interactionRanges; set => interactionRanges = value; }
    }
}
