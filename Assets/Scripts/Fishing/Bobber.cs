using System;
using Interfaces;
using UnityEngine;

namespace Fishing
{
    public class Bobber : MonoBehaviour
    {
        public Rigidbody rb;
        [SerializeField]private LayerMask fishingSpotLayer;
        public float radius;


        public bool isBusy;

        public bool castedOut;
        // Declare hitColliders as a reusable field.
        private Collider[] hitColliders;

        // Set the maximum number of colliders that can be detected at once.
        private const int maxColliders = 5;


        void Start()
        {
            hitColliders = new Collider[maxColliders];
        }

        void FixedUpdate()
        {
            if (isBusy) return;
            
            if(!castedOut) return;

            int numCollidersFound =
                Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, fishingSpotLayer);
            
            if (numCollidersFound > 0)
            {
                Debug.Log("Hit");
                rb.linearVelocity = Vector3.zero;
                
                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < numCollidersFound; i++)
                {
                    if (hitColliders[i].TryGetComponent(out IBobberInteractive bobber))
                    {
                        bobber.OnBobberEnter();
                        isBusy = true;
                        
                        break;
                    }
                }
                
                isBusy = true;
            }
        }
        
        private void OnDrawGizmos()

        {
            Gizmos.color = Color.blanchedAlmond;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        // void OnTriggerEnter(Collider other)
     // {
     //     if (other.CompareTag("FishingSpot"))
     //     {
     //         other.GetComponent<FishingSpot>().StartFishing();
     //         Debug.Log("Doing SomeFishing");
     //     }

     //     rb.linearVelocity = Vector3.zero;
     // }
    }
}