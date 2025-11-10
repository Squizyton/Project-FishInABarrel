using Player;
using Service_Locator;

namespace Fishing
{
    public class FishingSpot
    {
        private void Start()
        {
            
        }


        private void StartFishing()
        {
            ServiceLocator.Instance.Locate(out PlayerMovement movement);
            movement.ChangeState(PlayerMovement.State.Fishing);
            
            
            //Load Fishing Scene
        }
    }
}