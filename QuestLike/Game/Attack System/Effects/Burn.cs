using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestLike.Combat;
using QuestLike.Organs;

namespace QuestLike.Effects
{
    class Burn : Effect
    {

        public Burn() : base("Burning", new string[] { "burn", "burning" })
        {
            AddAction<IDamagable>(OnDamagableAdded, OnDamagableUpdate, OnDamagableRemoved);
            uses = 5;
        }

        public Burn(string name, string[] ids) : base(name, ids)
        {
        }

        public Burn(string name, uint priority, string[] ids) : base(name, priority, ids)
        {
        }

        public Burn(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Burn(string name, uint priority, string desc, string[] ids) : base(name, priority, desc, ids)
        {
        }

        public Burn(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public Burn(string name, uint priority, string shortdesc, string desc, string[] ids) : base(name, priority, shortdesc, desc, ids)
        {
        }

        public void OnDamagableAdded(GameObject part)
        {
        }

        public void OnDamagableUpdate(GameObject part)
        {
            var damageable = part as IDamagable;
            damageable.OnDamage(new DamageInfo()
            {
                damage = 5,
                damageType = DamageType.magical,
                magicType = MagicType.fire,
                sender = sender,
                target = part
            });
        }

        public void OnDamagableRemoved(GameObject part)
        {

        }

    }
}
