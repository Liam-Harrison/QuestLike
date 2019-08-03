using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Organs;
using QuestLike.Combat;
using QuestLike.State;
using SadConsole;
using Console = SadConsole.Console;
using Global = SadConsole.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Input;
using QuestLike;
using QuestLike.Entities;


namespace QuestLike
{
    class Program
    {
        static GameState state;
        static void Main(string[] args)
      {
            Settings.LoadSettings();

            SadConsole.Game.Create(Settings.Width, Settings.Height);
            SadConsole.Game.Instance.Window.Title = "QuestLike";

            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;

            SwitchState(new PlayingState());

            Room testRoom = new Room("Surgery");
            testRoom.PrefillEdgesWithWalls();

            testRoom.SetRoom(new char[][] {
                new char[] { },
                new char[] {' ', ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] {' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] {' ', ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] {' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] {'#', ' ', ' ', '#', '#', ' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] {' ', ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] {' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] {' ', ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] {' ', ' ', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ',' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                new char[] { },
            });

            testRoom.GetCollection<GameObject>().AddObject(new Entity("Bear", 
                new string[] { "bear" })
                { screenChar = 'B', position = new Point(2, 9) });

            testRoom.GetCollection<GameObject>().AddObject(new Item("Sword", 
                new string[] { "sword" })
                { screenChar = 's', position = new Point(1, 1) });

            testRoom.GetCollection<GameObject>().AddObject(new Weapon("Gun", 
                new string[] { "gun" })
                { screenChar = 'g', position = new Point(1, 1) });

            testRoom.GetCollection<GameObject>().AddObject(new Equipable<Organs.Chest>("Leather Chestpiece", 
                new string[] { "leather chestpiece", "chestpiece" })
                { screenChar = 'l', position = new Point(2, 4) });

            var npc = new HumanoidNPC("The Surgeon", new string[] { "npc", "the surgeon", "surgeon" })
                { screenChar = 'S', position = new Point(16, 5), screenCell = new Cell(Color.Red) };
            npc.SetNPCRoutine(
                (entity) => {
                    entity.GetOwner.Move(Pathfinding.RandomDirection());
                });

            Action<ITalkable> start = null;

            Action<ITalkable> second = (ITalkable entity) =>
            {
                entity.Say("Did you misunderstand me or somethin'?");
                Utilities.PromptSelection<string>("Reponse", new string[] { "yes", "no?" }, (index, canceled) =>
                {
                    if (int.TryParse(index, out int result))
                    {
                        if (result == 0)
                        {
                            entity.Say("Watch yourself");
                            entity.SetTalkRoutine(start);
                        }
                        else
                        {
                            entity.Say("... Idiot");
                            entity.SetTalkRoutine(start);
                        }
                    }
                });
            };

            start = (ITalkable first) => {
                first.Say("Don't distrub me!");
                first.SetTalkRoutine(second);
            };

            npc.SetTalkRoutine(start);
            npc.SetNameColor(Color.Red);
            testRoom.GetCollection<GameObject>().AddObject(npc);

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

        private static void Update(GameTime gametime)
        {
            if (GameScreen.miniConsole != null) GameScreen.miniConsole.UpdateScreen(gametime);
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
            SadConsole.Themes.Library.Default.ButtonTheme = new SadConsole.Themes.ButtonLinesTheme
            {
                EndCharacterLeft = '|',
                EndCharacterRight = '|'
            };

            var console = new SadConsole.ControlsConsole(Settings.Width, Settings.Height)
            {
                DefaultBackground = Color.Black,
                DefaultForeground = Color.White
            };

            console.PrintCentre(Settings.Width / 2, 1, "Main  Screen");

            console.Add(new NavigateButton<GameScreen>(16, 3)
            {
                Text = "Play",
                Position = new Point(Settings.Width / 2 - 8, 6)
            });

            console.Add(new NavigateButton<OptionsScreen>(16, 3)
            {
                Text = "Options",
                Position = new Point(Settings.Width / 2 - 8, 10)
            });

            console.Add(new NavigateButton<InfoScreen>(16, 3)
            {
                Text = "Information",
                Position = new Point(Settings.Width / 2 - 8, 14)
            });

            var exit = new SadConsole.Controls.Button(16, 3)
            {
                Text = "Exit Game",
                Position = new Point(Settings.Width / 2 - 8, Settings.Height - 8)
            };
            exit.Click += (a, b) =>
            {
                Environment.Exit(0);
            };
            console.Add(exit);

            console.IsFocused = true;
            SadConsole.Global.CurrentScreen = console;

            GameScreen.PrintLine($"\nNeed some help? You can type <{Color.Cyan.ToInteger()},help>Help@ at any time to see all the commands you can use.");
            GameScreen.Print($" You can also enable <{Color.Orange.ToInteger()},>[info]@ messages and debug commands by enabling debug mode in the options menu.");
            GameScreen.PrintLine($"\nWhen you see a link <{Color.Cyan.ToInteger()},>like this@ you can click on it to quickly interact with objects or repeat commands.");
        }
    }
}
