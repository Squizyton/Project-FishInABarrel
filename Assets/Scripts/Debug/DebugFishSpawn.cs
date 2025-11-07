using Service_Locator;
using UnityEngine;

public class DebugFishSpawn : MonoBehaviour, IOnInteract
{
    public Fish fishPrefab;
    public int amountToSpawn;
    public Transform spawnPoint;
    public void OnInteract()
    {
        
        var test = ServiceLocator.Instance.Locate<GameManager>(out var gameManager);
        
        for (int i = 0; i < amountToSpawn; i++)
        {
            var spawnedFish = Instantiate(fishPrefab, spawnPoint.transform.position + Random.insideUnitSphere, Quaternion.identity);
            spawnedFish.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }
}
