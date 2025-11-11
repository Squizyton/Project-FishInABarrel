using UnityEngine;

namespace Fishing
{
    public class Bobber : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FishingSpot"))
            {
                other.GetComponent<FishingSpot>().StartFishing();
                Debug.Log("Doing SomeFishing");
            }
        }
    }
}