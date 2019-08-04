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
using Newtonsoft.Json;

[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
class Weapon : Item, IUseable
{
    public Weapon() : base()
    {

    }

    public Weapon(string name, string[] ids) : base(name, ids)
    {
    }

    public Weapon(string name, string desc, string[] ids) : base(name, desc, ids)
    {
    }

    public Weapon(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
    {
    }

    public virtual void Use(GameObject sender, GameObject target, object[] arguments = null)
    {
        GameScreen.PrintLine($"\nUsing {Name}");
    }
}