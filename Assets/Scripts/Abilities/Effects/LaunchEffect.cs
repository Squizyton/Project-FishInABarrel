using System;
using Abilities.Interfaces;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Abilities.Effects
{
    [Serializable]
    public class LaunchEffect : IEffect<IDamageable>
    {
        [SerializeField] private float procChance;

        public void OnUpdateTick(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void OnHit(IDamageable target)
        {
            float chanceNumber = Random.Range(0f, 1f);

            if (chanceNumber < procChance)
            {
                var hasRb = ((Transform)target).TryGetComponent<Rigidbody>(out Rigidbody rb);
                
                if (hasRb)
                {
                    rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
                }
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