using UnityEngine;

namespace Pooling
{
    public interface IPoolableObject
    {
        public bool IsAvailable { get; set; }

        void OnActivate();

        
        void OnDeactivate();
    }
}
