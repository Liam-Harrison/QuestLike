using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Organs;
using ZorkLike.Combat;
using ZorkLike.State;
using SadConsole;
using Console = SadConsole.Console;
using Global = SadConsole.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Input;
using ZorkLike;


namespace ZorkLike
{
    class Program
    {
        public static int Width = 125, Height = 35;
        public static int MinWidth = 75, MinHeight = 20;
        public static int MaxWidth = 200, MaxHeight = 60;

        static GameState state;
        static void Main(string[] args)
        {
            SadConsole.Game.Create(Width, Height);
            SadConsole.Game.Instance.Window.Title = "QuestLike";

            SadConsole.Game.OnInitialize = Init;

            SwitchState(new PlayingState());

            Room testRoom = new Room("Room");

            testRoom.AddCollection<GameObject>();
            testRoom.AddCollection<Item>();

            testRoom.GetCollection<GameObject>().AddObject(new Entity("Bear", new string[] { "bear" }));
            testRoom.GetCollection<GameObject>().AddObject(new Entity("Frog", new string[] { "frog" }));
            testRoom.GetCollection<GameObject>().AddObject(new Item("Sword", new string[] { "sword" }));
            testRoom.GetCollection<GameObject>().AddObject(new Item("Gun", new string[] { "gun" }));

            Game.AddRoom(testRoom);
            Game.ChangeRoom(testRoom);

            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        static void SwitchState(GameState newState)
        {
            if (state != null) state.OnExit();
            state = newState;
            state.OnEnter();
        }

        private static void Init()
        {
            var colors = SadConsole.Themes.Library.Default.Colors.Clone();
            colors.Appearance_ControlNormal = new Cell(Color.White, Color.Black);
            colors.Appearance_ControlFocused = new Cell(Color.White, Color.Gray);
            colors.Appearance_ControlMouseDown = new Cell(Color.White, Color.DarkGray);
            colors.Appearance_ControlOver = new Cell(Color.White, Color.Gray);
            colors.Appearance_ControlSelected = new Cell(Color.White, Color.Gray);
            colors.TextLight = Color.White;
            colors.ControlBack = Color.Black;
            colors.ControlHostBack = Color.Black;
            colors.Text = Color.White;
            colors.TitleText = Color.White;

            SadConsole.Themes.Library.Default.Colors = colors;
            SadConsole.Themes.Library.Default.ButtonTheme = new SadConsole.Themes.ButtonLinesTheme();
            SadConsole.Themes.Library.Default.ButtonTheme.EndCharacterLeft = '|';
            SadConsole.Themes.Library.Default.ButtonTheme.EndCharacterRight = '|';

            var console = new SadConsole.ControlsConsole(Width, Height);

            console.DefaultBackground = Color.Black;
            console.DefaultForeground = Color.White;

            console.PrintCentre(Width / 2, 1, "Main  Screen");

            console.Add(new NavigateButton<GameScreen>(16, 3)
            {
                Text = "Play",
                Position = new Point(Width / 2 - 8, 6)
            });

            console.Add(new NavigateButton<OptionsScreen>(16, 3)
            {
                Text = "Options",
                Position = new Point(Width / 2 - 8, 10)
            });

            console.Add(new SadConsole.Controls.Button(16, 3)
            {
                Text = "Information",
                Position = new Point(Width / 2 - 8, 14)
            });

            console.IsFocused = true;
            SadConsole.Global.CurrentScreen = console;

            GameScreen.PrintLine($"\nNeed some help? You can type <{Color.Cyan.ToInteger()},help>Help@ at any time to see all the commands you can use.");
            GameScreen.Print($" You can also enable <{Color.Orange.ToInteger()},>[info]@ messages and debug commands by enabling debug mode in the options menu.");
            GameScreen.PrintLine($"\nWhen you see a link <{Color.Cyan.ToInteger()},>like this@ you can click on it to quickly interact with objects or repeat commands.");
        }
    }
}
