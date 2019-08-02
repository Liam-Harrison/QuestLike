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
    public enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    };

    public static class Pathfinding
    {

        public static Point DirectionToPoint(Direction direction)
        {
            var stringname = direction.ToString();
            var point = new Point(0, 0);

            if (stringname.Contains("North")) point.Y = -1;
            else if (stringname.Contains("South")) point.Y = 1;

            if (stringname.Contains("East")) point.X = 1;
            else if (stringname.Contains("West")) point.X = -1;

            return point;
        }

        public class BFSPoint
        {
            public Point point;
            public bool searched;
            public int xDistance;
            public int yDistance;
        }

        private static List<BFSPoint> points = new List<BFSPoint>();
        public static BFSPoint[] SpillGetPoints(Point start, int xRange, int yRange, bool diagonalPathing = true, bool honorBlockables = true)
        {
            points.Clear();

            CheckPoint(start, 0, 0, xRange, yRange, diagonalPathing, honorBlockables);

            return points.ToArray();
        }

        public static BFSPoint[] SubtractBFSLists(BFSPoint[] left, BFSPoint[] right)
        {
            List<BFSPoint> larger = new List<BFSPoint>(left);

            foreach (var leftBFS in larger.ToArray())
            {
                foreach (var rightBFS in right)
                {
                    if (leftBFS.point == rightBFS.point)
                    {
                        larger.Remove(leftBFS);
                        continue;
                    }
                }
            }

            return larger.ToArray();
        }

        public static Point[] GetExteriorPoints(BFSPoint[] bfspoints, bool honorblockables = true)
        {
            List<Point> points = new List<Point>();

            foreach (var point in bfspoints)
            {
                foreach (var position in GetValidAdjacentPoints(point.point, true, honorblockables))
                {
                    if (!IsPointInBFSList(position, bfspoints))
                    {
                        points.Add(position);
                    }
                }
            }

            return points.ToArray();
        }

        public static bool IsPointInBFSList(Point point, BFSPoint[] points)
        {
            foreach (var subpoint in points)
            {
                if (subpoint.point == point)
                {
                    return true;
                }
            }
            return false;
        }

        private static void CheckPoint(Point position, int xDistance, int yDistance, int xRange, int yRange, bool diagonalPathing = true, bool honorBlocking = true)
        {
            var found = points.Find((a) => { return a.point == position; });
            if (found == null)
            {
                if (xDistance > xRange) return;
                if (yDistance > yRange) return;
                points.Add(new BFSPoint() { point = position, searched = true, xDistance = xDistance, yDistance = yDistance});
                var adjacents = GetValidAdjacentPoints(position, diagonalPathing, honorBlocking);
                foreach (var item in adjacents)
                {
                    var delta = item - position;
                    CheckPoint(item, xDistance + Math.Abs(delta.X), yDistance + Math.Abs(delta.Y), xRange, yRange);
                }
            }
            else
            {
                if (xDistance > xRange) return;
                if (yDistance > yRange) return;
                if (xDistance < found.xDistance || yDistance < found.yDistance)
                {
                    points.Remove(found);
                    points.Add(new BFSPoint() { point = position, searched = true, xDistance = xDistance, yDistance = yDistance });
                    var adjacents = GetValidAdjacentPoints(position, diagonalPathing, honorBlocking);
                    foreach (var item in adjacents)
                    {
                        var delta = item - position;
                        CheckPoint(item, xDistance + Math.Abs(delta.X), yDistance + Math.Abs(delta.Y), xRange, yRange);
                    }
                }
            }
        }

        public static bool IsNextToInvalidPoint(Point point, bool diagonalPathing = true, bool honorBlocking = true)
        {
            List<Point> points = new List<Point>();

            var north = DirectionToPoint(Direction.North) + point;
            var east = DirectionToPoint(Direction.East) + point;
            var south = DirectionToPoint(Direction.South) + point;
            var west = DirectionToPoint(Direction.West) + point;

            if (!MiniScreen.IsValidScreenPosition(north, honorBlocking)) return true;
            if (!MiniScreen.IsValidScreenPosition(east, honorBlocking)) return true;
            if (!MiniScreen.IsValidScreenPosition(south, honorBlocking)) return true;
            if (!MiniScreen.IsValidScreenPosition(west, honorBlocking)) return true;

            if (diagonalPathing)
            {
                var northeast = DirectionToPoint(Direction.NorthEast) + point;
                var southeast = DirectionToPoint(Direction.SouthEast) + point;
                var southwest = DirectionToPoint(Direction.SouthWest) + point;
                var northwest = DirectionToPoint(Direction.NorthWest) + point;

                if (!MiniScreen.IsValidScreenPosition(northeast, honorBlocking)) return true;
                if (!MiniScreen.IsValidScreenPosition(southeast, honorBlocking)) return true;
                if (!MiniScreen.IsValidScreenPosition(southwest, honorBlocking)) return true;
                if (!MiniScreen.IsValidScreenPosition(northwest, honorBlocking)) return true;
            }

            return false;
        }

        public static Point[] GetValidAdjacentPoints(Point point, bool diagonalPathing = true, bool honorBlocking = true)
        {
            List<Point> points = new List<Point>();
            
            var north = DirectionToPoint(Direction.North) + point;
            var east = DirectionToPoint(Direction.East) + point;
            var south = DirectionToPoint(Direction.South) + point;
            var west = DirectionToPoint(Direction.West) + point;

            if (MiniScreen.IsValidScreenPosition(north, honorBlocking)) points.Add(north);
            if (MiniScreen.IsValidScreenPosition(east, honorBlocking)) points.Add(east);
            if (MiniScreen.IsValidScreenPosition(south, honorBlocking)) points.Add(south);
            if (MiniScreen.IsValidScreenPosition(west, honorBlocking)) points.Add(west);

            if (diagonalPathing)
            {
                var northeast = DirectionToPoint(Direction.NorthEast) + point;
                var southeast = DirectionToPoint(Direction.SouthEast) + point;
                var southwest = DirectionToPoint(Direction.SouthWest) + point;
                var northwest = DirectionToPoint(Direction.NorthWest) + point;

                if (MiniScreen.IsValidScreenPosition(northeast, honorBlocking)) points.Add(northeast);
                if (MiniScreen.IsValidScreenPosition(southeast, honorBlocking)) points.Add(southeast);
                if (MiniScreen.IsValidScreenPosition(southwest, honorBlocking)) points.Add(southwest);
                if (MiniScreen.IsValidScreenPosition(northwest, honorBlocking)) points.Add(northwest);
            }

            return points.ToArray();
        }
    }
}
