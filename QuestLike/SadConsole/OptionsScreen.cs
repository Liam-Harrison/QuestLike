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

static class Settings
{
    public static int Width = 100, Height = 25;
    public static int MinWidth = 75, MinHeight = 20;
    public static int MaxWidth = 200, MaxHeight = 60;
    public static bool DebugMode = false;
}

class OptionsScreen : SadConsole.ControlsConsole
{
    public OptionsScreen() : base(Program.Width, Program.Height)
    {
        Generate();
    }

    private void Generate()
    {
        DefaultBackground = Color.Black;
        DefaultForeground = Color.White;

        this.PrintCentre(Width / 2, 1, "Options");

        this.PrintCentre(Width / 2, 4, "Game Size");

        this.PrintCentre(Width / 2, 6, "                  ");
        this.PrintCentre(Width / 2, 6, "Width (" + Settings.Width + ")");
        Print(Width / 2 - 18, 7, Program.MinWidth.ToString());
        Print(Width / 2 + 16, 7, Program.MaxWidth.ToString());
        var widthScroll = new SadConsole.Controls.ScrollBar(Orientation.Horizontal, 30) { Position = new Point(Width / 2 - 15, 7) };
        widthScroll.Maximum = Settings.MaxWidth - Settings.MinWidth;
        widthScroll.Step = 1;
        widthScroll.Value = Settings.Width - Settings.MinWidth;
        widthScroll.ValueChanged += WidthScroll_ValueChanged;
        Add(widthScroll);
        
        this.PrintCentre(Width / 2, 9, "                  ");
        this.PrintCentre(Width / 2, 9, "Height (" + Settings.Height + ")");
        Print(Width / 2 - 18, 10, Program.MinHeight.ToString());
        Print(Width / 2 + 16, 10, Program.MaxHeight.ToString());
        var heightScroll = new SadConsole.Controls.ScrollBar(Orientation.Horizontal, 30) { Position = new Point(Width / 2 - 15, 10) };
        heightScroll.Maximum = Settings.MaxHeight - Settings.MinHeight;
        heightScroll.Step = 1;
        heightScroll.Value = Settings.Height - Settings.MinHeight;
        heightScroll.ValueChanged += HeightScroll_ValueChanged;
        Add(heightScroll);

        this.PrintCentre(Width / 2, 14, "Game Settings");

        var debug = new SadConsole.Controls.CheckBox(15, 1) { Text = "Debug Mode", Position = new Point(Width / 2 - 7, 16) };
        debug.IsSelected = Settings.DebugMode;
        debug.IsSelectedChanged += (a, b) => 
        {
            Settings.DebugMode = ((a as SadConsole.Controls.CheckBox).IsSelected);
            if (Settings.DebugMode)
            {
                GameScreen.PrintLine($"\nDebug mode enabled - You can simulate the bodyparts on yourself to test the blood and organs systems with the <{Color.Cyan.ToInteger()},debug simulate me>debug simulate me@ command.");
                GameScreen.Print($"You can also get a summary of all your bodyparts with the <{Color.Cyan.ToInteger()},debug look at me>debug look at me@ command.");
            }
        };
        Add(debug);

        Add(new BackButton(30, 3)
        {
            Text = "Discard changes and return",
            Position = new Point(Width / 2 - 32, Program.Height - 8)
        });

        Add(new BackButton(30, 3)
        {
            Text = "Save changes and return",
            Position = new Point(Width / 2 + 2, Program.Height - 8)
        });

    }

    private void HeightScroll_ValueChanged(object sender, EventArgs e)
    {
        this.PrintCentre(Width / 2, 9, "                  ");
        this.PrintCentre(Width / 2, 9, "Height (" + ((sender as SadConsole.Controls.ScrollBar).Value + Program.MinHeight) + ")");
        this.PrintCentre(Width / 2, 12, "Resolution changes will require a restart", new Cell(Color.OrangeRed, Color.Black));
    }

    private void WidthScroll_ValueChanged(object sender, EventArgs e)
    {
        this.PrintCentre(Width / 2, 6, "                  ");
        this.PrintCentre(Width / 2, 6, "Width (" + ((sender as SadConsole.Controls.ScrollBar).Value + Program.MinWidth) + ")");
        this.PrintCentre(Width / 2, 12, "Resolution changes will require a restart", new Cell(Color.OrangeRed, Color.Black));
    }
}
