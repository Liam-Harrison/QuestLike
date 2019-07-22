using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZorkLike.Combat;

namespace ZorkLike.Effects
{
    class Spell : GameObject, IUseable
    {
        public DamageType damageType;
        public MagicType magicType;
        public float damage;
        public Effect[] effects;

        public Spell(string name, string[] ids) : base(name, ids)
        {
        }

        public Spell(string name, string desc, string[] ids) : base(name, desc, ids)
        {
        }

        public Spell(string name, string shortdesc, string desc, string[] ids) : base(name, shortdesc, desc, ids)
        {
        }

        public virtual void Use(GameObject sender, GameObject target, object[] arguments = null)
        {
            if (target is IEffectable)
            {
                foreach (var effect in effects)
                {
                    ((IEffectable)target).AddEffect(effect);
                }
            }

            if (target is IDamagable)
            {
                ((IDamagable)target).OnDamage(
                    new DamageInfo
                    {
                        damage = damage,
                        magicType = magicType,
                        damageType = damageType,
                        sender = sender,
                        target = target
                    }
                );
            }
        }
    }
}
