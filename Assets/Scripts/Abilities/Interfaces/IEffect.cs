namespace Abilities.Interfaces
{
    public interface IEffect<TTarget>
    {
        void OnUpdateTick(float deltaTime);
        void OnHit(TTarget target);
        void OnReleod();
        void OnDeath(TTarget target);
    }
}
