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

        public void AddResponse(string response, Action<ITalkable> newroutine)
        {
            ((ITalkable)dialogueManager).AddResponse(response, newroutine);
        }

        public void ClearResponses()
        {
            ((ITalkable)dialogueManager).ClearResponses();
        }

        public void DrawResponses()
        {
            ((ITalkable)dialogueManager).DrawResponses();
        }

        public string GetName()
        {
            return ((ITalkable)dialogueManager).GetName();
        }

        public void NPCUpdate()
        {
            ((INPC)npcManager).NPCUpdate();
        }

        public void OnReponse(int index)
        {
            ((ITalkable)dialogueManager).OnReponse(index);
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
    void AddResponse(string response, Action<ITalkable> newroutine);
    void ClearResponses();
    void Say(string text, bool sign = true);
    void SetNameColor(Color color);
    void DrawResponses();
    void OnReponse(int index);
    string GetName();
    Entity GetOwner { get; }
}

class DialogueManager : ITalkable
{
    private Entity owner;
    private List<string> reponses = new List<string>();
    public string[] Responses { get => reponses.ToArray(); set => reponses = new List<string>(value); }

    public DialogueManager(Entity owner)
    {
        if (owner as ITalkable == null) throw new Exception("You cannot have this component on an object which does not implement its interface.");
        this.owner = owner;
    }

    public Action<ITalkable> ontalk = null;
    public virtual void TalkTo()
    {
        ontalk?.Invoke(this);
        DrawResponses();
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

    public void DrawResponses()
    {
        if (reponses.Count == 0) return;

        string text = "";

        for (int i = 0; i < Responses.Length; i++)
        {
            text += $"\n\t <{Color.Orange.ToInteger()},respond {owner.ID} x {i}>{reponses[i]}@";
        }

        GameScreen.PrintLine(text);
    }

    public void OnReponse(int index)
    {
        onresponse[index]?.Invoke(this);
    }

    public Dictionary<int, Action<ITalkable>> onresponse = new Dictionary<int, Action<ITalkable>>();
    public void SetResponseRoutine(int index, Action<ITalkable> newroutine)
    {
        onresponse.Add(index, newroutine);
    }

    public void ClearResponses()
    {
        reponses.Clear();
        onresponse.Clear();
    }

    public void AddResponse(string response, Action<ITalkable> newroutine)
    {
        reponses.Add(response);
        onresponse.Add(reponses.IndexOf(response), newroutine);
    }
}

class NPCManager: INPC
{
    private Entity owner;

    public NPCManager(Entity owner)
    {
        if (owner as INPC == null) throw new Exception("You cannot have this component on an object which does not implement its interface.");
        this.owner = owner;
    }

    public Entity GetOwner => owner;

    Action<INPC> onupdate = null;
    public void SetNPCRoutine(Action<INPC> newroutine)
    {
        onupdate = newroutine;
    }

    public void NPCUpdate()
    {
        onupdate?.Invoke(owner as INPC);
    }

}