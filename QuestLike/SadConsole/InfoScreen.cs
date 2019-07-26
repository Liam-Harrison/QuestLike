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

class InfoScreen : ControlsConsole
{
    public InfoScreen() : base(Program.Width, Program.Height)
    {
        Generate();
    }

    public InfoScreen(int width, int height) : base(width, height)
    {
        Generate();
    }

    private void Generate()
    {
        DefaultBackground = Color.Black;
        DefaultForeground = Color.White;

        Add(new BackButton(30, 3)
        {
            Text = "Back to Main Menu",
            Position = new Point(Width / 2 - 15, Program.Height - 8)
        });

        this.PrintCentre(Width / 2, 6, "QuestLike");

        this.PrintCentre(Width / 2, 8, "Developed By".Pad(22) + "Liam Harrison");
        this.PrintCentre(Width / 2, 10, "liamharrison.io");

        this.PrintCentre(Width / 2, 16, "Graphics by SadConsole");
        this.PrintCentre(Width / 2, 18, "SadConsole Developed By".Pad(28) + "Thraka");
        this.PrintCentre(Width / 2, 20, "https://sadconsole.com/");
    }
}
