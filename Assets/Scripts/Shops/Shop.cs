using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour, IOnInteract
{
    
    private uint _shopId;
    
    [Title("Shop Options")] [SerializeField]
    private int amountOfItems;

    private readonly HashSet<BuyableItem> _currentItems = new HashSet<BuyableItem>();


    public enum ShopType
    {
        GunShop,
        FishShop,
        UpgradeShop
    }


    private void Start()
    {
        _shopId = (uint)Random.Range(0, uint.MaxValue);
    }

    public void OnInteract()
    {
        OpenShop();
    }
    
    protected virtual void OpenShop()
    {
        if (_currentItems.Count == 0)
        {
            //Generate some items
            for (int i = 0; i < amountOfItems; i++)
            {
                //Test
                _currentItems.Add(new BuyableItem {id = i, price = Random.Range(500,5000)});
            }
        }
    }

    public virtual void BuyItem(int id)
    {
        //Get the item from the list
        var item = _currentItems.FirstOrDefault(i => i.id == id);
    }

    public virtual void CloseShop()
    {
    }
    
    public virtual void ChangeAmountOfItems(int amount)
    {
        amountOfItems  += amount;
    }


    public List<BuyableItem> GetItems()
    {
        return _currentItems.ToList();
    }

}


public struct BuyableItem
{
    public int id;
    public int price;
    public Sprite icon;
    public string name;
    public string description;
}