using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Effects
{
    public class Effector: IEffectable
    {
        private List<Effect> effects = new List<Effect>();

        private GameObject owner;
        public Effector(GameObject owner)
        {
            this.owner = owner;
        }

        public void AddEffect(Effect effect)
        {
            effects.Add(effect);
            effect.OnAdded(owner);
            effects.Sort((a, b) => { return a.CompareTo(b); });
        }

        public void RemoveEffect(Effect effect)
        {
            effects.Remove(effect);
            effect.OnRemoved(owner);
        }

        public string Examine
        {
            get
            {
                if (effects.Count == 0) return "";

                string text = "\n\n";

                text += "Currently being effected by " + effects.Count + " effects.";

                foreach (var effect in effects)
                {
                    if (effect.ShortDescription == "") text += "\n - " + effect.Name;
                    else text += "\n - " + effect.Name + " - " + effect.ShortDescription;
                }

                return text;
            }
        }

        public bool EffectCanBeUsedOn(Effect effect)
        {
            return effect.ImplmentsMe(owner);
        }

        public bool HasEffect(Effect effect)
        {
            foreach (var i in effects)
            {
                if (effect == i) return true;
            }
            return false;
        }
        
        public void UpdateEffects()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].OnUpdate(owner);
                if (effects[i].uses > 0) effects[i].uses--;
                if (effects[i].uses == 0)
                {
                    effects[i].OnRemoved(owner);
                    effects.RemoveAt(i);
                }
            }
        }
    }
}
