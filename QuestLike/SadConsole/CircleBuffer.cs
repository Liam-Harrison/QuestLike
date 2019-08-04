using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole;
using Console = SadConsole.Console;
using Global = SadConsole.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Input;
using Newtonsoft.Json;

[JsonObject]
public class CircleBuffer<T>: IEnumerable<T>, IEnumerable
{
    [JsonProperty]
    Queue<T> contents;
    [JsonProperty]
    int size = 0;
    public CircleBuffer(int size)
    {
        this.size = size;
        contents = new Queue<T>(size);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return ((IEnumerable<T>)contents).GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable<T>)contents).GetEnumerator();
    }

    public T GetAtIndex(int index)
    {
        return contents.ElementAtOrDefault(index);
    }

    public void Push(T item)
    {
        if (contents.Count() >= size)
        {
            contents.Dequeue();
        }
        contents.Enqueue(item);
    }

    public T Last()
    {
        return contents.LastOrDefault();
    }

    public void EditLast(T newContent)
    {
        contents = new Queue<T>(contents.Take(contents.Count - 1));
        contents.Enqueue(newContent);
    }
}

public static class Extensions
{
    public static void PrintCentre(this Console console, int x, int y, string text, Cell cell = null)
    {
        int half_text_width = text.Count() / 2;
        if (cell == null) console.Print(x - half_text_width, y, text, new Cell(Color.White, Color.Black));
        else console.Print(x - half_text_width, y, text, cell);
    }

    public static void PrintCentre(this ControlsConsole console, int x, int y, string text, Cell cell = null)
    {
        int half_text_width = text.Count() / 2;
        if (cell == null) console.Print(x - half_text_width, y, text, new Cell(Color.White, Color.Black));
        else console.Print(x - half_text_width, y, text, cell);
    }
}