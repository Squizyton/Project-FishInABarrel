using System;
using Interfaces;
using UnityEngine;

namespace Damage
{
    [Serializable]
    public class BaseDamageProcessor : DamageProcessor
    {
        public override void ProcessDamage(IDamageable target, DamageData damageData)
        {
            var obj = target as MonoBehaviour;
         
            target.OnDamage(damageData.Damage);
        }
    }
}