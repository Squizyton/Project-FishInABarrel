using System;
using Interfaces;

namespace Damage
{
    [Serializable]
    public class DamageProcessor : IDamageProcessor<IDamageable>
    {
        private IDamageProcessor<IDamageable> _next;
        private bool HasNext => _next != null;
        
        public virtual void ProcessDamage(IDamageable target ,DamageData damageData)
        {
            if (HasNext) _next.ProcessDamage(target,damageData);
        }

        public virtual IDamageProcessor<IDamageable> SetNext(IDamageProcessor<IDamageable> next)
        {
            return _next = next;
        }
        
    }
}