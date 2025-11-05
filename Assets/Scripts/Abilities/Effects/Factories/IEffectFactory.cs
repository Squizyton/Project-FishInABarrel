using Abilities.Interfaces;

namespace Abilities.Effects.Factories
{
    public interface IEffectFactory<TTarget>
    {
        IEffect<TTarget> Create();
    }
}