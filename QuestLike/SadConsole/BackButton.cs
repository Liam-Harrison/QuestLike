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

class BackButton : SadConsole.Controls.Button
{
    public BackButton(int width, int height) : base(width, height)
    {
    }

    public override void DoClick()
    {
        GameWindow.NavigateBack();
    }
}
