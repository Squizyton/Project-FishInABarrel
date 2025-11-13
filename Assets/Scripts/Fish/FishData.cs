using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish/New Fish")]
public class FishData : ScriptableObject
{
    public string nameOfFish;
    public Sprite sprite;
    [Title("Prefabs")]
    
    public GameObject inWaterFishPrefab;
    public Fish outOfWaterFishPrefab;
    
    
    [Title("Stats")]
    public float weight;
    
    
    //Movement behavior can go here
}
