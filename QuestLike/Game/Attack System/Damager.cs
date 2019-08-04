using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestLike.Combat
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Damager : IDamagable
    {
        [JsonProperty]
        private float damage = 0;
        [JsonProperty]
        private List<DamageInfo> history = new List<DamageInfo>();

        public Damager()
        {
        }

        [JsonIgnore]
        public float Damage
        {
            get
            {
                return damage;
            }
        }

        [JsonIgnore]
        public DamageLevel DamageLevel
        {
            get
            {
                if (damage >= 0 && damage < 10)
                    return DamageLevel.Perfect;
                else if (damage >= 10 && damage < 30)
                    return DamageLevel.Fine;
                else if (damage >= 30 && damage < 50)
                    return DamageLevel.Bruised;
                else if (damage >= 50 && damage < 70)
                    return DamageLevel.Horrible;
                else if (damage >= 70 && damage < 90)
                    return DamageLevel.Failing;
                return DamageLevel.Destroyed;
            }
        }

        public void OnDamage(DamageInfo damageInfo)
        {
            history.Add(damageInfo);
            damage = Utilities.Clamp(damage + damageInfo.damage, 0, 100);
        }
    }
}
