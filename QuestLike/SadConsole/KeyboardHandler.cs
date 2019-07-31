using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Components;
using SadConsole.Input;
using Microsoft.Xna.Framework;
using SadConsole;
using QuestLike.Entities;

using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace QuestLike
{
    class KeyboardHandler : KeyboardConsoleComponent
    {
        private List<Keys> registeredpresses = new List<Microsoft.Xna.Framework.Input.Keys>();

        public override void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
        {
            handled = false;

            RegisterMoveKey(info, Keys.Up, Direction.North);
            RegisterMoveKey(info, Keys.Right, Direction.East);
            RegisterMoveKey(info, Keys.Left, Direction.West);
            RegisterMoveKey(info, Keys.Down, Direction.South);

            foreach (var key in registeredpresses.ToArray())
            {
                if (info.IsKeyUp(key)) registeredpresses.Remove(key);
            }
        }

        private void RegisterMoveKey(Keyboard info, Keys key, Direction direction)
        {
            if (info.IsKeyDown(key) && !registeredpresses.Contains(key))
            {
                registeredpresses.Add(key);
                Game.GetPlayer.Move(direction);
            }
        }
    }
}
