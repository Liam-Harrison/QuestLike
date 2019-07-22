using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Effects
{

    abstract public class Effect: GameObject, IComparable<Effect>
    {
        public uint priority;
        public int uses = -1;
        public GameObject sender;
        public delegate void EffectAction(GameObject effectable);

        public Effect(string name, string[] ids) : base(name, ids)
        {
        }

        public Effect(string name, uint priority, string[] ids) : base(name, ids)
        {
            this.priority = priority;
        }

        public Effect(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Effect(string name, uint priority, string desc, string[] ids) : base(name, desc, ids)
        {
            this.priority = priority;
        }

        public Effect(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public Effect(string name, uint priority, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
            this.priority = priority;
        }

        public int CompareTo(Effect other)
        {
            if (this == null || other == null) return 0;

            return priority.CompareTo(other.priority);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Effect)) return false;
            Effect effect = obj as Effect;
            if (Name == effect.Name) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Effect left, Effect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Effect left, Effect right)
        {
            return !(left == right);
        }

        protected void AddAction<T>(EffectAction onAdded, EffectAction onUpdate, EffectAction onRemoved)
        {
            actions.Add((typeof(T), onAdded, onUpdate, onRemoved));
        }

        public void OnAdded(GameObject effectable)
        {
            foreach (var action in actions)
            {
                if (effectable.GetType() == action.Item1 || effectable.GetType().IsSubclassOf(action.Item1))
                {
                    if (action.Item2 != null) action.Item2.Invoke(effectable);
                }
            }
        }

        public void OnUpdate(GameObject effectable)
        {
            foreach (var action in actions)
            {
                if (action.Item1.IsAssignableFrom(effectable.GetType()) || effectable.GetType() == action.Item1 || effectable.GetType().IsSubclassOf(action.Item1))
                {
                    if (action.Item3 != null) action.Item3.Invoke(effectable);
                }
            }
        }

        public void OnRemoved(GameObject effectable)
        {
            foreach (var action in actions)
            {
                if (effectable.GetType() == action.Item1 || effectable.GetType().IsSubclassOf(action.Item1))
                {
                    if (action.Item4 != null) action.Item4.Invoke(effectable);
                }
            }
        }

        protected List<(Type, EffectAction, EffectAction, EffectAction)> actions = new List<(Type, EffectAction, EffectAction, EffectAction)>();
        public bool ImplmentsMe(object obj)
        {
            foreach (var action in actions)
            {
                if (obj.GetType().IsInstanceOfType(action.Item1)) return true;
            }
            return false;
        }
    }
}
