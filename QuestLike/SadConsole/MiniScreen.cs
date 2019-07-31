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

class MiniScreen: SadConsole.ControlsConsole
{
    public MiniScreen() : base(24, 10)
    {
        Position = new Point(Settings.Width - 24, 0);
    }

    private List<GameObject> miniscreenObjects = new List<GameObject>();

    public void UpdateScreen(GameTime time)
    {
        Clear();
        UpdateMiniScreenObjects();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var objects = GetAllObjectsAtPoint(new Point(x, y));
                if (objects.Count == 0) continue;
                else if (objects.Count == 1) PrintObject(objects.First());
                else
                {
                    var clickrate = objects.Count;
                    var index = (int) time.TotalGameTime.TotalSeconds % clickrate;
                    PrintObject(objects[index]);
                }
            }
        }
    }

    public void PrintObject(GameObject gameObject)
    {
        if (miniscreenObjects.Contains(gameObject))
        {
            Print(gameObject.ScreenPosition.X,
                  gameObject.ScreenPosition.Y,
                  gameObject.ScreenChar);
        }
    }

    public void UpdateMiniScreenObjects()
    {
        miniscreenObjects.Clear();
        miniscreenObjects.Add(QuestLike.Game.GetPlayer);
        foreach (var gameobject in QuestLike.Game.GetRoom.LocateObjectsWithType<GameObject>())
        {
            if (!gameobject.PrintOnScreen) continue;
            miniscreenObjects.Add(gameobject);
        }
    }

    public List<GameObject> GetAllObjectsAtPoint(Point point)
    {
        List<GameObject> gameobjects = new List<GameObject>();
        foreach (var gameobject in miniscreenObjects)
        {
            if (point == gameobject.ScreenPosition) gameobjects.Add(gameobject);
        }
        return gameobjects;
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
