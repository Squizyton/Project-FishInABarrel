using System;
using Abilities.Interfaces;
using Interfaces;

namespace Abilities.Effects.Factories
{
    [Serializable]
    public class DamageEffectFactory : IEffectFactory<IDamageable>
    {
        public IEffect<IDamageable> Create()
        {
            return new DamageEffect();
        }
    }

    [Serializable]
    public class LaunchEffectFactory : IEffectFactory<IDamageable>
    {
        public IEffect<IDamageable> Create()
        {
            return new LaunchEffect();
        }
    }


}