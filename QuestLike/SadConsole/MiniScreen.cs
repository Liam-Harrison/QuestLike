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

class MiniScreen: SadConsole.ControlsConsole
{
    private static int ScreenWidth = 24, ScreenHeight = 11;
    public MiniScreen() : base(ScreenWidth, ScreenHeight)
    {
        Position = new Point(Settings.Width - ScreenWidth, 1);
    }

    private List<GameObject> miniscreenObjects = new List<GameObject>();

    public void UpdateScreen(GameTime time)
    {
        Clear();
        UpdateMiniScreenObjects();

        for (int y = 0; y < ScreenHeight; y++)
        {
            for (int x = 0; x < ScreenWidth; x++)
            {
                var objects = GetAllObjectsAtPoint(new Point(x, y));
                if (objects.Count == 0)
                {
                    if (IsRangeWallAtPoint(new Point(x, y))) {
                        PrintObject(RangeWallAtPoint(new Point(x, y)));
                    }
                    continue;
                }
                else if (objects.Count == 1) PrintObject(objects.First());
                else
                {
                    var switchrate = objects.Count;
                    var modifier = switchrate - 1;
                    if (modifier > 3) modifier = 3;
                    var index = (int) (time.TotalGameTime.TotalSeconds * modifier) % objects.Count;
                    PrintObject(objects[index]);
                }
            }
        }

        DrawPlayerMoveArea();
    }

    public static bool IsValidScreenPosition(Point point, bool honorBlocking = true)
    {
        var valid = point.X >= 0 && point.Y >= 0 && point.X < ScreenWidth && point.Y < ScreenHeight;
        if (!valid) return false;
        var objects = GameScreen.miniConsole.GetAllObjectsAtPoint(point);
        foreach (var item in objects)
        {
            if (honorBlocking && item.blocking) return false;
        }
        return true;
    }

    public List<RangeWall> rangewalls = new List<RangeWall>();
    public void DrawPlayerMoveArea()
    {
        foreach (var rangewall in rangewalls)
            miniscreenObjects.Remove(rangewall);

        rangewalls.Clear();
        var exterior = Pathfinding.GetExteriorPoints(Game.GetPlayer.MoveArea);

        foreach (var item in exterior)
        {
            rangewalls.Add(new RangeWall() { position = item });
        }
    }

    public void PrintObject(GameObject gameObject)
    {
        if (miniscreenObjects.Contains(gameObject) || rangewalls.Contains(gameObject))
        {
            if (gameObject.ScreenCell == null)
                Print(gameObject.Position.X,
                  gameObject.Position.Y,
                  gameObject.ScreenChar);
            else
                Print(gameObject.Position.X,
                  gameObject.Position.Y,
                  gameObject.ScreenChar,
                  gameObject.ScreenCell);
        }
    }

    public void UpdateMiniScreenObjects()
    {
        miniscreenObjects.Clear();
        miniscreenObjects.Add(QuestLike.Game.GetPlayer);
        foreach (var gameobject in QuestLike.Game.GetRoom.LocateObjectsWithType<GameObject>(true, true))
        {
            if (!IsValidScreenPosition(gameobject.Position)) continue;
            miniscreenObjects.Add(gameobject);
        }
        foreach (var gameobject in QuestLike.Game.GetRoom.LocateObjectsWithType<Wall>(true, true))
        {
            if (!IsValidScreenPosition(gameobject.Position)) continue;
            miniscreenObjects.Add(gameobject);
        }
    }

    public List<GameObject> GetAllObjectsAtPoint(Point point)
    {
        List<GameObject> gameobjects = new List<GameObject>();
        foreach (var gameobject in miniscreenObjects)
        {
            if (point == gameobject.Position) gameobjects.Add(gameobject);
        }
        return gameobjects;
    }

    public bool IsRangeWallAtPoint(Point point)
    {
        foreach (var gameobject in rangewalls)
        {
            if (gameobject.Position == point) return true;
        }
        return false;
    }

    public RangeWall RangeWallAtPoint(Point point)
    {
        foreach (var gameobject in rangewalls)
        {
            if (gameobject.Position == point) return gameobject;
        }
        return null;
    }

    protected override void OnMouseLeftClicked(MouseConsoleState state)
    {
        base.OnMouseLeftClicked(state);

        var objects = GetAllObjectsAtPoint(state.ConsoleCellPosition);

        if (objects.Count == 1) QuestLike.Game.ProcessInput($"look at {objects.First().ID}", true);
        else if (objects.Count > 1)
        {
            Utilities.PromptSelection<GameObject>($"There are {objects.Count} items at this position, what do you want to look at?", objects.ToArray(), (gameobject, canceled) =>
            {
                if (canceled) return;
                QuestLike.Game.ProcessInput($"look at {gameobject.ID}", true);
            });
        }
    }
}
