using System;
using System.Collections.Generic;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Global = SadConsole.Global;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Input;
using ZorkLike;

static class GameWindow
{
    public static Console Screen
    {
        get
        {
            if (screens.Count == 0) return SadConsole.Global.CurrentScreen;
            return screens.Peek();
        }
    }

    public static Console GlobalScreen
    {
        get
        {
            return SadConsole.Global.CurrentScreen;
        }
    }

    public static void NavigateTo<T>() where T: Console
    {
        var instance = Activator.CreateInstance<T>();
        Screen.Children.Add(instance);
        FocusOn(instance);
    }

    public static void AddScreen(Console screen)
    {
        Screen.Children.Add(screen);
    }

    public static bool CanNavigateBack()
    {
        return screens.Count > 0;
    }

    public static Stack<Console> screens = new Stack<Console>();
    public static void NavigateBack()
    {
        if (!CanNavigateBack()) return;
        var current = screens.Pop();
        var parent = Screen;
        parent.Children.Remove(current);

        if (screens.Count > 0)
        {
            parent.IsFocused = true;
        }
        else
        {
            GlobalScreen.IsFocused = true;
        }
    }

    public static void RebuildScreens()
    {
        var allScreens = new List<Console>();

    }

    public static void FocusOn(Console screen)
    {
        screens.Push(screen);
        screen.IsFocused = true;
    }
}
