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
    public void DrawMiniScreen()
    {
        miniscreenObjects.Clear();
        Clear();

        miniscreenObjects.Add(QuestLike.Game.GetPlayer);

        Print(QuestLike.Game.GetPlayer.ScreenPosition.X,
              QuestLike.Game.GetPlayer.ScreenPosition.Y,
              QuestLike.Game.GetPlayer.ScreenChar);

        foreach (var gameobject in QuestLike.Game.GetRoom.LocateObjectsWithType<GameObject>())
        {
            if (!gameobject.PrintOnScreen) continue;
            Print(gameobject.ScreenPosition.X,
                  gameobject.ScreenPosition.Y,
                  gameobject.ScreenChar);
            miniscreenObjects.Add(gameobject);
        }
    }

    protected override void OnMouseLeftClicked(MouseConsoleState state)
    {
        base.OnMouseLeftClicked(state);

        foreach (var gameobject in miniscreenObjects)
        {
            if (gameobject.ScreenPosition == state.ConsoleCellPosition)
            {
                QuestLike.Game.ProcessInput($"look at {gameobject.ID}", true);
                return;
            }
        }
    }
}
