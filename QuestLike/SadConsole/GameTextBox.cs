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

class GameTextBox : SadConsole.Controls.TextBox
{
    string placeholder;
    public GameTextBox(int width) : base(width)
    {
        placeholder = "Input a sentence";
        Text = placeholder;
    }

    protected override void OnLeftMouseClicked(MouseConsoleState state)
    {
        base.OnLeftMouseClicked(state);
    }

    public char[] allowedList =
    {
        'a','b','c','d','e','f','g','h',
        'i','j','k','l','m','n','o','p',
        'q','r','s','t','u','v','w','x',
        'y','z',' ','1','2','3','4','5',
        '6','7','8','9','0'
    };
    public override bool ProcessKeyboard(Keyboard info)
    {
        foreach (var key in info.KeysPressed)
        {
            if (allowedList.Contains(key.Character.ToString().ToLower()[0]) && Text.Length <= Settings.Width - 28)
            {
                Text += key.Character;
            }
            else if (key.Key == Microsoft.Xna.Framework.Input.Keys.Back && Text.Length > 0)
            {
                Text = Text.Remove(Text.Length - 1, 1);
            }
        }
        foreach (var key in info.KeysDown)
        {
            if (key.Key == Microsoft.Xna.Framework.Input.Keys.Enter && Text.Length > 0)
            {
                if (GameScreen.buffer.Count() > 0) GameScreen.NewLine();
                GameScreen.PrintLine($"> <{Color.Cyan.ToInteger()},{Text.ToLower()}>{Text.ToLower()}@");
                ZorkLike.Game.ProcessInput(Text.ToLower());
                Text = "";
                GameScreen.UpdateScrollBar();
                Focused();
            }
        }

        return true;
    }

    public override void FocusLost()
    {
        base.FocusLost();

        Text = placeholder;
    }

    public override void Focused()
    {
        base.Focused();

        Text = "";
    }
}