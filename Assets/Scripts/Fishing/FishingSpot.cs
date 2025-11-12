using System.Collections.Generic;
using Player;
using Service_Locator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fishing
{
    public class FishingSpot: MonoBehaviour
    {
        private FishingManager _fishingManager;


        [SerializeField]private List<FishData> fishThatCanSpawn;
        
        private void Start()
        {
            _fishingManager = FindAnyObjectByType<FishingManager>();
        }


        public void StartFishing()
        {
            ServiceLocator.Instance.Locate(out PlayerMovement movement);
            movement.ChangeState(PlayerMovement.State.Fishing);
            SceneManager.LoadSceneAsync("FishingScene", LoadSceneMode.Additive).completed += operation =>
            {
                _fishingManager.StartFishing(this, ref fishThatCanSpawn);
            };
        }
    }
}