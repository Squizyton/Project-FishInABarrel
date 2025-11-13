using Guns;
using Service_Locator;
using UnityEngine;

public class DebugGunPedastal : MonoBehaviour, IOnInteract
{

    [SerializeField] private BaseGun gunToGivePlayer;


    public void OnInteract()
    {
         GameManager.Instance.inventory.AddToolToInventory(gunToGivePlayer);
    }
}
