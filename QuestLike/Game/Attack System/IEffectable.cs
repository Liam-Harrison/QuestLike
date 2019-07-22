using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Effects
{
    public interface IEffectable
    {
        void AddEffect(Effect effect);
        bool HasEffect(Effect effect);
        bool EffectCanBeUsedOn(Effect effect);
        void UpdateEffects();
    }
}
