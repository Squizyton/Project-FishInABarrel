using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class MinigameBobber : MonoBehaviour
    {
        [SerializeField] private FishingMiniGame miniGameManager;
        [SerializeField] private Transform fishCaughtSpot;
        
        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{other.transform.name}");
            if (other.CompareTag("Fish"))
            {

                if (miniGameManager.OnFishCaught(other.transform))
                {
                    other.transform.position = fishCaughtSpot.position;
                    other.transform.SetParent(fishCaughtSpot);
                }
            }
        }


    }
}