using System.Collections.Generic;
using Alchemy.Inspector;
using Player;
using UnityEngine;
using Utilities.Utilities;

public class GameManager : SingletonBehaviour<GameManager>
{
    [Title("References")] public PlayerInventory inventory;


    [Title("Lists")] [SerializeField] private HashSet<Fish> fishes = new HashSet<Fish>();


 
}