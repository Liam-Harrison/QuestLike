using Microsoft.Xna.Framework;
using QuestLike;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestLike.Entities
{
    class HumanoidNPC : Humanoid, INPC, ITalkable
    {
        private DialogueManager dialogueManager;
        private NPCManager npcManager;

        public HumanoidNPC() : base()
        {

        }

        public HumanoidNPC(string name, string[] ids) : base(name, ids)
        {
            dialogueManager = new DialogueManager(this);
            npcManager = new NPCManager(this);
        }

        public Entity GetOwner => ((ITalkable)dialogueManager).GetOwner;

        public string GetName()
        {
            return ((ITalkable)dialogueManager).GetName();
        }

        public void NPCUpdate()
        {
            ((INPC)npcManager).NPCUpdate();
        }

        public void Say(string text, bool sign = true)
        {
            ((ITalkable)dialogueManager).Say(text, sign);
        }

        public void SetNameColor(Color color)
        {
            ((ITalkable)dialogueManager).SetNameColor(color);
        }

        public void SetNPCRoutine(Action<INPC> newroutine)
        {
            ((INPC)npcManager).SetNPCRoutine(newroutine);
        }

        public void SetTalkRoutine(Action<ITalkable> newroutine)
        {
            ((ITalkable)dialogueManager).SetTalkRoutine(newroutine);
        }

        public void TalkTo()
        {
            ((ITalkable)dialogueManager).TalkTo();
        }

        public override void Update()
        {
            base.Update();

            npcManager.NPCUpdate();
        }
    }
}

interface INPC
{
    void NPCUpdate();
    void SetNPCRoutine(Action<INPC> newroutine);
    Entity GetOwner { get; }
}

interface ITalkable
{
    void TalkTo();
    void SetTalkRoutine(Action<ITalkable> newroutine);
    void Say(string text, bool sign = true);
    void SetNameColor(Color color);
    string GetName();
    Entity GetOwner { get; }
}

class DialogueManager: ITalkable
{
    private Entity owner;
    public DialogueManager(Entity owner)
    {
        if (owner as ITalkable == null) throw new Exception("You cannot have this component on an object which does not implement its interface.");
        this.owner = owner;
        ontalk = owner.talkaction;
    }

    int state = 0;
    Action<ITalkable> ontalk;
    public virtual void TalkTo()
    {

    }

    public void SetTalkRoutine(Action<ITalkable> newroutine)
    {
        ontalk = newroutine;
    }

    public void Say(string text, bool sign = true)
    {
        GameScreen.NewLine();
        if (sign)
        {
            GameScreen.PrintLine(GetName() + ":\n");
        }
        GameScreen.PrintLine(text);
    }

    private Color nameColor;

    public Entity GetOwner => owner;

    public void SetNameColor(Color color)
    {
        nameColor = color;
    }

    public string GetName()
    {
        if (nameColor == Color.Transparent) return owner.Name;
        return $"<{nameColor.ToInteger()},look at {owner.ID}>{owner.Name}@";
    }
}

class NPCManager: INPC
{
    private Entity owner;

    public NPCManager(Entity owner)
    {
        if (owner as INPC == null) throw new Exception("You cannot have this component on an object which does not implement its interface.");
        this.owner = owner;
        onupdate = owner.onupdate;
    }

    public Entity GetOwner => owner;

    Action<INPC> onupdate;
    public void SetNPCRoutine(Action<INPC> newroutine)
    {
        onupdate = newroutine;
    }

    public void NPCUpdate()
    {
        if (onupdate != null) onupdate.Invoke(owner as INPC);
    }

}