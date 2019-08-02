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
        private Dictionary<Keys, double> registeredpresses = new Dictionary<Keys, double>();

        public override void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
        {
            handled = false;

            RegisterMoveKey(info, Keys.Up, Direction.North);
            RegisterMoveKey(info, Keys.Right, Direction.East);
            RegisterMoveKey(info, Keys.Left, Direction.West);
            RegisterMoveKey(info, Keys.Down, Direction.South);

            foreach (var register in registeredpresses.ToArray())
            {
                if (info.IsKeyUp(register.Key)) registeredpresses.Remove(register.Key);
                else if (Global.GameTimeUpdate.TotalGameTime.TotalSeconds - register.Value > 0.15f) registeredpresses.Remove(register.Key);
            }
        }

        private void RegisterMoveKey(Keyboard info, Keys key, Direction direction)
        {
            if (info.IsKeyDown(key) && !registeredpresses.ContainsKey(key))
            {
                registeredpresses.Add(key, Global.GameTimeUpdate.TotalGameTime.TotalSeconds);
                Game.GetPlayer.Move(direction);
            }
        }
    }
}
