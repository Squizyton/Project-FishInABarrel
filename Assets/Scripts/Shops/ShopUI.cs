using System.Collections.Generic;
using Alchemy.Inspector;
using Shops.UIElements;
using UnityEngine;

namespace Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Transform optionsTransform;

        public int totalNumberOfRecipesPerStation;

        public Shop.ShopType currentTypeOfShop;


        [Title("Shop UI Prefabs", "Handles the overall Look and Feel of the Shop UI")] [SerializeField]
        public Dictionary<Shop.ShopType, ShopFeelAndLook> shopUIPrefabs;

        [Title("Shop UI Containers", "Handles the individual UI for each shop")] [SerializeField]
        public Dictionary<Shop.ShopType, GameObject> CurrentShopUIs;

        [Title("Shop Items")] 
        private Dictionary<ShopIDAndBuyableItems, List<SlotUI>> shopItems;


        public void OpenShop(Shop.ShopType shopType, int shopID, Shop shopAccessing)
        {
            currentTypeOfShop = shopType;

            foreach (var shop in CurrentShopUIs)
                shop.Value.SetActive(false);

            foreach (Transform value in optionsTransform.transform)
            {
                value.gameObject.SetActive(false);
            }

            CurrentShopUIs[shopType].SetActive(true);


            ShopIDAndBuyableItems? shopIDAndShopType = null;

            //Loop through all the keys and set the slots associated with that shop to true
            foreach (var shop in shopItems.Keys)
            {
                if (shop.shopID == shopID && shop.shopType == shopType)
                {
                    shopIDAndShopType = shop;
                }
            }


            if (!shopIDAndShopType.HasValue)
            {
                //3 attempts to fill a crafting station
                int attemptsRemaining = 5;

                var buyableItems = shopAccessing.GetItems();
                
                for (int i = 0; i < buyableItems.Count; i++)
                {
                    var buyableItem = buyableItems[i];
                    
                    
                    //Create a base UI Element
                    var buyableSlot = Instantiate(shopUIPrefabs[shopType].baseSlotUIPrefab, optionsTransform);
                    
                    
                    shopItems.Add(new ShopIDAndBuyableItems(shopType, shopID), new List<SlotUI>());
                }
            }
            else
            {
                foreach(var item in shopItems[shopIDAndShopType.Value])
                    item.gameObject.SetActive(true);
            }
            
        }
    }


    public struct ShopFeelAndLook
    {
        public GameObject shopContainer;
        public BaseUIElement baseSlotUIPrefab;
    }

    public struct ShopIDAndBuyableItems
    {
        public int shopID;
        public Shop.ShopType shopType;

        public ShopIDAndBuyableItems(Shop.ShopType shopType, int shopID)
        {
            this.shopType = shopType;
            this.shopID = shopID;
        }
    }
}