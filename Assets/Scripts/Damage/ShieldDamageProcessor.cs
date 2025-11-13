using Damage;
using Interfaces;
using UnityEngine;

public class ShieldDamageProcessor : DamageProcessor
{
    private readonly float _amountOfShield;
    
    public ShieldDamageProcessor(float amountOfShield)
    {
           _amountOfShield = amountOfShield;
    }


    //TODO:: Current design is that you destroy shield before enemy takes damage? Do I just want shield to regenerate?
    public override void ProcessDamage(IDamageable target, DamageData damageData)
    {
        if (_amountOfShield <= 0)
        {
            base.ProcessDamage(target,damageData);
            return;
        }
        
        
        var mutableDamage = damageData;

        mutableDamage.Damage -= _amountOfShield;

        if (damageData.Damage > 0)
            base.ProcessDamage(target, mutableDamage);
        else
            Debug.Log($"Shield took {damageData.Damage} damage");


    }
}
