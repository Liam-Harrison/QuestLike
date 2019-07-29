using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Combat
{
    public enum DamageType
    {
        physical,
        mixed,
        magical
    }

    public enum MagicType
    {
        none,
        arcane,
        holy,
        unholy,
        fire,
        frost,
        pestilence,
        destruction,
        demonology,
        control
    }

    public enum WeaponType
    {
        slicing,
        blunt,
        piercing
    }

    public struct DamageInfo
    {
        public GameObject sender;
        public GameObject target;
        public WeaponType weaponType;
        public DamageType damageType;
        public MagicType magicType;
        public float damage;
    }

    interface IDamagable
    {
        void OnDamage(DamageInfo damageInfo);
        float Damage { get; }
        DamageLevel DamageLevel { get; }

    }
}
