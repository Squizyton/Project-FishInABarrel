using Player;
using Service_Locator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fishing
{
    public class FishingSpot: MonoBehaviour
    {
        private void Start()
        {
            
        }


        public void StartFishing()
        {
            ServiceLocator.Instance.Locate(out PlayerMovement movement);
            movement.ChangeState(PlayerMovement.State.Fishing);
            SceneManager.LoadSceneAsync("Fishing", LoadSceneMode.Additive);
        }
    }
}