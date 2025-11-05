using System;
using Interfaces;
using UnityEngine;

namespace Damage.Factories
{
    
    public interface IProcessorFactory<TTarget>
    {
             IDamageProcessor<TTarget> Create();
    }
    
    [Serializable]
    //A factory for the BaseDamageprocessor
    public class BaseDamageFactory : IProcessorFactory<IDamageable>
    {
        public IDamageProcessor<IDamageable> Create()
        {
            return new BaseDamageProcessor();
        }
    }

    [Serializable]
    public class ShieldDamageFactory : IProcessorFactory<IDamageable>
    {
        public float amountOfShield;
        public IDamageProcessor<IDamageable> Create()
        {
            return new ShieldDamageProcessor(amountOfShield);
        }
    }

}