using Alchemy.Inspector;
using Tools.Fishing_Rods;
using UnityEngine;

namespace Fishing
{
    public class FishingManager : MonoBehaviour
    {
        [Title("Current Round Stats")]
        [SerializeField] private int currentFishCaught;

        [Title("Current Rod")] [SerializeField]
        private FishingRod currentFishingRod;
        
        
        
        
    }
}
