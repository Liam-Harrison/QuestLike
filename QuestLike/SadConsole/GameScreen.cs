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

public class GameScreen : SadConsole.ControlsConsole
{
    public GameScreen() : base(Settings.Width, Settings.Height)
    {
        Generate();
    }

    public GameScreen(int width, int height) : base(width, height)
    {

    }

    public static void Print(string text)
    {
        PrintLine(text, true, true, null);
    }

    public static int ActualLength(string text)
    {
        int length = 0;
        bool count = true;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n' || text[i] == '\r') continue;
            else if (text[i] == '<')
            {
                count = false;
                continue;
            }
            else if (text[i] == '>')
            {
                count = true;
                continue;
            }
            else if (text[i] == '@')
            {
                continue;
            }
            if (count) length++;
        }
        return length;
    }

    public static string ActualText(string text)
    {
        string toReturn = "";
        bool count = true;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n' || text[i] == '\r') continue;
            else if (text[i] == '<')
            {
                count = false;
                continue;
            }
            else if (text[i] == '>')
            {
                count = true;
                continue;
            }
            else if (text[i] == '@')
            {
                continue;
            }
            if (count) toReturn += text[i];
        }
        return toReturn;
    }

    /// <summary>
    /// Prints text to the games scrolling console.
    /// </summary>
    /// <param name="text">The text you want to print.</param>
    /// <param name="editLastLine">Do you want to edit the last line in the buffer?</param>
    /// <param name="writeToBuffer">Write this to the buffer?</param>
    /// <param name="linkoverflow">Used internally when printing links overflow.</param>
    public static void PrintLine(string text, bool editLastLine = false, bool writeToBuffer = true, (Color, string)? linkoverflow = null)
    {
        int charCounter = 0;
        string lastbuffer = buffer.Last();

        if (buffer.Count() > 0 && editLastLine)
        {
            charCounter = ActualLength(lastbuffer);
            if (lastbuffer.EndsWith("\n\r")) lastbuffer = lastbuffer.Remove(lastbuffer.Count() - 2);
            text = lastbuffer + text;
            if (gameConsole != null)
            {
                if (gameConsole.Cursor.Position.Y == 0) GameScreen.gameConsole.Cursor.Position = new Point(charCounter, 0);
                else GameScreen.gameConsole.Cursor.Position = new Point(charCounter, gameConsole.Cursor.Position.Y - 1);
            }
        }

        if (linkoverflow.HasValue) text = ("<" + linkoverflow.Value.Item1.ToInteger() + "," + linkoverflow.Value.Item2 + ">") + text;
        (Color, string)? link = null;
        bool readingLink = false;
        if (linkoverflow.HasValue)
        {
            link = linkoverflow.Value;
            readingLink = true;
        }
        for (int i = (buffer.Count() > 0 && editLastLine ? lastbuffer.Length : 0); i < text.Length; i++)
        {
            if (WillNextWordBeCut(charCounter, text.Substring(i)))
            {
                WrapText(text, editLastLine, writeToBuffer, link, readingLink, i);
                return;
            }
            else if (text[i] == '@')
            {
                readingLink = false;
                link = null;
                continue;
            }
            else if (text[i] == '<')
            {
                link = ExtractLink(text, i, out int newIndex);
                i = newIndex;
                links.Push(new Link()
                {
                    points = new List<Point>(),
                    text = link.Value.Item2
                });
                readingLink = true;
                continue;
            }
            else if (text[i] == '\n')
            {
                if (writeToBuffer && editLastLine) buffer.EditLast(text.Substring(0, i) + "\n");
                else if (writeToBuffer) buffer.Push(text.Substring(0, i) + "\n");

                if (gameConsole != null) gameConsole.Cursor.Print("\n\r");
                if (text.Length >= i + 1)
                {
                    string remainder = text.Substring(i + 1);
                    PrintLine(remainder, false, writeToBuffer, link);
                }
                UpdateScrollBar();
                return;
            }
            else if (text[i] == '\r')
            {
                gameConsole.Cursor.Print("\r");
            }
            else if (readingLink)
            {
                if (gameConsole != null)
                {
                    var original = gameConsole.Cursor.PrintAppearance;
                    gameConsole.Cursor.PrintAppearance = new Cell(link.Value.Item1);
                    links.Last().points.Add(gameConsole.Cursor.Position);
                    gameConsole.Cursor.Print(text[i].ToString());
                    gameConsole.Cursor.PrintAppearance = original;
                }
                charCounter++;
            }
            else if (!readingLink)
            {
                if (gameConsole != null) gameConsole.Cursor.Print(text[i].ToString());
                charCounter++;
            }

        }
        if (gameConsole != null && writeToBuffer) gameConsole.Cursor.Print("\n\r");

        if (writeToBuffer && editLastLine) buffer.EditLast(text + "\n\r");
        else if (writeToBuffer) buffer.Push(text + "\n\r");

        UpdateScrollBar();
    }

    private static void WrapText(string text, bool editLastLine, bool writeToBuffer, (Color, string)? link, bool readingLink, int i)
    {
        string printed = text.Substring(0, i);
        string remainder = text.Substring(i);
        remainder = remainder.TrimStart(); // remove any overflowing starting whitespaces.

        if (writeToBuffer && editLastLine) buffer.EditLast(printed + (readingLink ? "@" : "") + "\n\r");
        else if (writeToBuffer) buffer.Push(printed + (readingLink ? "@" : "") + "\n\r");

        if (gameConsole != null) gameConsole.Cursor.Print("\n\r");
        PrintLine(remainder, false, writeToBuffer, link);
        UpdateScrollBar();
        return;
    }

    public static bool WillNextWordBeCut(int charcounter, string remainingtext)
    {
        var text = ActualText(remainingtext);
        if (text.Length == 0) return false;
        var split = text.Split(' ');
        var lookingat = split.FirstOrDefault();
        for (int i = 0; i < lookingat.Length; i++)
        {
            if (i + charcounter >= Settings.Width - 29) return true;
        }
        return false;
    }

    public static (Color, string) ExtractLink(string rawText, int startIndex, out int newIndex)
    {
        newIndex = 0;
        string interior = "";
        for (int i = startIndex + 1; i < rawText.Length; i++)
        {
            if (rawText[i] == '>')
            {
                newIndex = i;
                break;
            }
            interior += rawText[i];
        }
        var split = interior.Split(',');
        Color color = new Color(uint.Parse(split[0]));
        string command = split[1];
        return (color, command);
    }

    public static void NewLine()
    {
        buffer.Push("\n\r");
        if (gameConsole != null) gameConsole.Cursor.Print("\n\r");
    }

    public static void UpdateScrollBar()
    {
        if (scrollbar == null) return;
        if (buffer.Count() < Settings.Height - 2)
        {
            scrollbar.IsVisible = false;
            scrollbar.IsEnabled = false;
            scrollbar.Value = 0;
        }
        else
        {
            scrollbar.IsVisible = true;
            scrollbar.IsEnabled = true;
            scrollbar.Maximum = buffer.Count() - (Settings.Height - 3);
            scrollbar.Value = buffer.Count() - (Settings.Height - 3);
        }
    }

    public static void LoadBuffer()
    {
        links = new CircleBuffer<Link>(100);
        gameConsole.Cursor.Position = new Point(0, 0);
        foreach (string line in buffer)
        {
            PrintLine(line, false, false, null);
        }
        UpdateScrollBar();
    }

    public static void ClearScreen()
    {
        buffer = new CircleBuffer<string>(200);
        links = new CircleBuffer<Link>(100);
        gameConsole.Clear();
        gameConsole.ClearShiftValues();
        gameConsole.Cursor.Position = new Point(0, 0);
        UpdateScrollBar();
    }

    public class Link
    {
        public string text;
        public List<Point> points;
    }

    public static CircleBuffer<string> buffer = new CircleBuffer<string>(200);
    public static ScrollingConsole gameConsole;
    internal static MiniScreen miniConsole;
    public static SadConsole.Controls.ScrollBar scrollbar;
    public static CircleBuffer<Link> links = new CircleBuffer<Link>(100);
    public static ControlsConsole infoconsole;
    public static ControlsConsole sidebarconsole;
    public static Console roomconsole;

    private void Generate()
    {
        DefaultBackground = Color.Black;
        DefaultForeground = Color.White;

        /// Theme Setup

        var themeNoSides = new SadConsole.Themes.ButtonTheme
        {
            ShowEnds = false
        };

        var theme = new SadConsole.Themes.ButtonTheme();

        /// Main Screen Area Construction

        var verticalBar = new Console(1, Settings.Height) { Position = new Point(Settings.Width - 25, 0), DefaultBackground = Color.LightGray };
        Children.Add(verticalBar);

        Add(new GameTextBox(Settings.Width - 25) { Position = new Point(0, Settings.Height - 1) });

        scrollbar = new SadConsole.Controls.ScrollBar(Orientation.Vertical, Settings.Height - 2) { Position = new Point(Settings.Width - 26, 1), Step = 1 };
        scrollbar.ValueChanged += Scroll_ValueChanged;
        Add(scrollbar);

        gameConsole = new ScrollingGameConsole(Settings.Width - 25, 200) { Position = new Point(0, 1) };
        gameConsole.ViewPort = new Rectangle(0, 0, Settings.Width - 26, Settings.Height - 2);
        gameConsole.DefaultBackground = Color.Black;
        Children.Add(gameConsole);

        this.PrintCentre((Width - 25) / 2, 0, "Game Screen");
        Add(new BackButton(17, 1) { Text = "Back to main menu", Position = new Point(0, 0), Theme = themeNoSides });

        LoadBuffer();

        /// Sidebar Construction & Placement

        sidebarconsole = new ControlsConsole(24, Settings.Height) { Position = new Point(Settings.Width - 24, 0) };

        roomconsole = new Console(sidebarconsole.Width, 1) { Position = new Point(0, 0), DefaultBackground = Color.LightGray };
        sidebarconsole.Children.Add(roomconsole);

        roomconsole.PrintCentre(sidebarconsole.Width / 2, 0, Game.GetRoom.Name, new Cell(Color.Black, Color.LightGray));

        var v2 = new Console(sidebarconsole.Width, 1) { Position = new Point(0, 12), DefaultBackground = Color.LightGray };
        sidebarconsole.Children.Add(v2);

        v2.PrintCentre(sidebarconsole.Width / 2, 0, "Stats", new Cell(Color.Black, Color.LightGray));

        infoconsole = new ControlsConsole(24, 5) { Position = new Point(0, 13) };

        infoconsole.Print(1, 1, $"{"Health".Pad(sidebarconsole.Width - 8)}0", new Cell(Color.White));
        infoconsole.Print(1, 2, $"{"Mana".Pad(sidebarconsole.Width - 8)}0", new Cell(Color.White));
        infoconsole.Print(1, 3, $"{"Action Points".Pad(sidebarconsole.Width - 8)}0/0", new Cell(Color.White));

        sidebarconsole.Children.Add(infoconsole);

        var v3 = new Console(sidebarconsole.Width, 1) { Position = new Point(0, 18), DefaultBackground = Color.LightGray };
        sidebarconsole.Children.Add(v3);

        v3.PrintCentre(sidebarconsole.Width / 2, 0, "Quick Commands", new Cell(Color.Black, Color.LightGray));

        var buttonInv = new SadConsole.Controls.Button(20, 1) { Text = "Inventory", Position = new Point(2, 20), Theme = theme };
        buttonInv.Click += (sender, e) => { QuestLike.Game.ProcessInput("look at my items"); };
        sidebarconsole.Add(buttonInv);

        var lookatroom = new SadConsole.Controls.Button(20, 1) { Text = "Look at room", Position = new Point(2, 22), Theme = theme };
        lookatroom.Click += (sender, e) => { QuestLike.Game.ProcessInput("look at room"); };
        sidebarconsole.Add(lookatroom);

        var clearbutton = new SadConsole.Controls.Button(20) { Text = "Clear Screen", Position = new Point(2, sidebarconsole.Height - 6), Theme = theme };
        clearbutton.Click += (a, b) => { ClearScreen(); };
        sidebarconsole.Add(clearbutton);

        var turn = new SadConsole.Controls.Button(sidebarconsole.Width - 4, 3)
        { Text = "Turn", Position = new Point(2, sidebarconsole.Height - 4), };
        turn.Click += (sender, e) => { QuestLike.Game.Update(); };
        sidebarconsole.Add(turn);

        Children.Add(sidebarconsole);

        /// Miniscreen Construction

        miniConsole = new MiniScreen();
        Children.Add(miniConsole);

        Components.Add(new KeyboardHandler());

    }

    private void Scroll_ValueChanged(object sender, EventArgs e)
    {
        var scroller = sender as SadConsole.Controls.ScrollBar;

        int offset = scroller.Value;
        gameConsole.ViewPort = new Rectangle(0, offset + 1, Settings.Width - 26, Settings.Height - 2);
    }
}
