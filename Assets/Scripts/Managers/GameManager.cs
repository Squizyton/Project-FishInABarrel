using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;
using Utilities.Utilities;

public class GameManager : SingletonBehaviour<GameManager>
{
    [Title("Game Stats")]
    [SerializeField] private double currentCash;

    
    
    [Title("Lists")]
    [SerializeField]private HashSet<Fish> fishes = new HashSet<Fish>();
    
    
    
    
    

    public void AddCash(float baseAmount, float mutliplyer)
    {
        float amountToAdd = baseAmount + (baseAmount * mutliplyer);
        
        currentCash += amountToAdd;
    }
    
}
