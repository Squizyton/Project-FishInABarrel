using System;
using System.Collections.Generic;
using Abilities.Effects.Factories;
using Interfaces;

namespace Abilities.Abilities
{
    
    [Serializable]
    public class DoubleDamage : Ability
    {

        public DoubleDamage()
        {
            effects = new List<IEffectFactory<IDamageable>>();
            
            effects.Add(new DamageEffectFactory());
            effects.Add(new LaunchEffectFactory());
        }

    }
}