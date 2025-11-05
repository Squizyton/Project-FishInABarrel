using System;
using Abilities.Interfaces;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Abilities.Effects
{
    [Serializable]
    public class DamageEffect : IEffect<IDamageable>
    {
        [SerializeField,Range(0,1)] private float procChance;
        
        public void OnUpdateTick(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void OnHit(IDamageable target)
        {
            float chanceNumber = Random.Range(0f, 1f);
            
            if (chanceNumber < procChance)
            {
                Debug.Log("Hit");
                //Pass in damage dealt here
                target.OnDamage(1);
            }
        }

        public void OnReleod()
        {
            throw new System.NotImplementedException();
        }

        public void OnDeath(IDamageable target)
        {
            throw new System.NotImplementedException();
        }

    }
}