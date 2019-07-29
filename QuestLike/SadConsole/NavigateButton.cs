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

class NavigateButton<T> : SadConsole.Controls.Button where T : SadConsole.Console
{
    public NavigateButton(int width, int height) : base(width, height)
    {
    }

    public override void DoClick()
    {
        GameWindow.NavigateTo<T>();
    }
}