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

class InfoScreen : ControlsConsole
{
    public InfoScreen() : base(Settings.Width, Settings.Height)
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
            Position = new Point(Width / 2 - 15, Settings.Height - 8)
        });

        this.PrintCentre(Width / 2, 6, "QuestLike");

        this.PrintCentre(Width / 2, 8, "Developed By: Liam Harrison");
        this.PrintCentre(Width / 2, 10, "https://liamharrison.io/");
        this.PrintCentre(Width / 2, 12, "https://github.com/Liam-Harrison/QuestLike");

        this.PrintCentre(Width / 2, 17, "Graphics by SadConsole");
        this.PrintCentre(Width / 2, 19, "SadConsole Developed By: Thraka");
        this.PrintCentre(Width / 2, 21, "https://sadconsole.com/");
    }
}
