using Structs;

namespace Interfaces
{
    public interface IHittable
    {
        void OnHit(HitInfo hitInfo);
    }
}