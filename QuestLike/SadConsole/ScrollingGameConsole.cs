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
using ZorkLike;

public class ScrollingGameConsole : SadConsole.ScrollingConsole
{
    public ScrollingGameConsole(int width, int height) : base(width, height)
    {
    }

    public ScrollingGameConsole(int width, int height, Font font) : base(width, height, font)
    {
    }

    public ScrollingGameConsole(int width, int height, Rectangle viewPort) : base(width, height, viewPort)
    {
    }

    public ScrollingGameConsole(int width, int height, Font font, Rectangle viewPort) : base(width, height, font, viewPort)
    {
    }

    public ScrollingGameConsole(int width, int height, Font font, Rectangle viewPort, Cell[] initialCells) : base(width, height, font, viewPort, initialCells)
    {
    }

    protected override void OnMouseLeftClicked(MouseConsoleState state)
    {
        base.OnMouseLeftClicked(state);

        var position = state.CellPosition;

        GameScreen.Link foundlink = null;
        foreach (GameScreen.Link link in GameScreen.links)
        {
            if (link.points.Contains(position))
            {
                foundlink = link;
                break;
            }
        }
        if (foundlink != null) ZorkLike.Game.ProcessInput(foundlink.text, true);
    }
}