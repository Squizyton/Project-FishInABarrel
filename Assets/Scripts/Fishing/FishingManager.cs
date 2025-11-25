using System.Collections.Generic;
using Alchemy.Inspector;
using Managers;
using Player;
using Service_Locator;
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



        void Start()
        {
            GameManager.Instance.inventory.OnToolChange += CheckForNewFishingRod;
        }

        public void CheckForNewFishingRod(PlayerInventory.EquippedTool newEquippedTool)
        {
            if(newEquippedTool.ToolReference is FishingRod fishingRod)
                currentFishingRod = fishingRod;
        }


        public void StartFishing(FishingSpot fishingSpot,ref List<FishData> fishThatCanSpawn)
        {
            var fishingMinigame = FindAnyObjectByType<FishingMiniGame>();
            
            ServiceLocator.Instance.Locate(out UIManager uiManager);
            uiManager.FishingStatus(true);
            
            ServiceLocator.Instance.Locate(out PlayerInteraction playerInteraction);
            
            
            
            //Start the fishing process
            fishingMinigame.StartGame(playerInteraction.CurrentRod, ref fishThatCanSpawn);
        }

        public void StopFishing(ref List<FishingMiniGame.CaughtFish> fishCollected)
        {
            
            ServiceLocator.Instance.Locate(out UIManager uiManager);
            ServiceLocator.Instance.Locate(out PlayerInteraction player);
            ServiceLocator.Instance.Locate(out PlayerMovement playerMovement);

            uiManager.FishingStatus(false);

            
            foreach (var caughtFish in fishCollected)
            {
                Fish spawnedFish = Instantiate(caughtFish.fishData.outOfWaterFishPrefab,player.CurrentRod.Bobber.transform.position + Random.insideUnitSphere + (Vector3.up * 2), Quaternion.identity);
                
                //Prime the fish
                StartCoroutine(spawnedFish.PrimeCountDown());
                
                if (spawnedFish.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddForce(Vector3.up * (10 / caughtFish.fishData.weight), ForceMode.Impulse);
                }
            }
            
            currentFishingRod.StopFishing();
            playerMovement.ChangeState(PlayerMovement.State.Walking);
        }
    }
}
