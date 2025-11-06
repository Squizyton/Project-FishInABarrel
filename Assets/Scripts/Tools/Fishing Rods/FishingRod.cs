using Alchemy.Inspector;
using UnityEngine;

namespace Tools.Fishing_Rods
{
    public class FishingRod : BasicTool
    {
        [Title("Stats")]
        [SerializeField] private float maxDepth;
        [SerializeField] private float speed;
        [SerializeField] private float maxFish;
    }
}