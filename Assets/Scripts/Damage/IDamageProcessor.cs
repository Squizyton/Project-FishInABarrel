namespace Damage
{
    public interface IDamageProcessor<TTarget>
    {
        void ProcessDamage(TTarget target,DamageData damageData);
        IDamageProcessor<TTarget> SetNext(IDamageProcessor<TTarget> next);
    }
}