using System;
using System.Collections.Generic;
using Abilities.Effects.Factories;
using Abilities.Interfaces;
using Alchemy.Inspector;
using Interfaces;
using UnityEngine;

namespace Abilities
{
    //TODO:: Maybe make Ability a data container and IEffect is what happens?
    [Serializable]
    //TODO ABILITY WILL PASS ALL THE EFFECTS TO THE THING IT HITS.
    //SO IF I HAVE A DOUBLEDAMAGE ABILITY
    // That will get passed from gun to Fish
    //Fish will then process effects
    // Ability could provide the targeting strategy, so if there is a chain hit, it will pass itself on to whereevcer it needs to go
    public class Ability
    {
        public enum EffectType
        {
            Enemy,
            Self,
            Weapon
        }

        /// <summary>
        /// Tells the effect what type of effect it is and how to sort it. 
        /// </summary>
        public EffectType effectType;

        [Title("Effects")] [SerializeReference]
        public List<IEffectFactory<IDamageable>> effects;

        public enum ExecutionType
        {
            OnTick,
            OnHit,
            OnDeath
        }
    }
}