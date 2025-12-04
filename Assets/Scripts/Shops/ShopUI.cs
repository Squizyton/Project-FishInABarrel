using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Shops.UIElements;
using UnityEngine;
using Alchemy.Serialization;
using Service_Locator;

namespace Shops
{
    [AlchemySerialize]
    public partial class ShopUI : MonoBehaviour, IService
    {
        [SerializeField] private Transform optionsTransform;

        public Shop.ShopType currentTypeOfShop;

        [Title("Shop UI Containers", "Handles the individual UI for each shop")] [AlchemySerializeField, NonSerialized]
        public Dictionary<Shop.ShopType, ShopFeelAndLook> CurrentShopUIs = new();

        [Title("Shop Items")] private Dictionary<ShopIDAndBuyableItems, List<SlotUI>> shopItems;


        void Start()
        {
            ServiceLocator.Instance.AddService(this);
            shopItems = new Dictionary<ShopIDAndBuyableItems, List<SlotUI>>();
        }


        private Dictionary<string, UIElement> _cacheBuildUIElements;

        public void OpenShop(Shop.ShopType shopType, uint shopID, Shop shopAccessing)
        {
            currentTypeOfShop = shopType;

            foreach (var shop in CurrentShopUIs)
                shop.Value.shopContainer.SetActive(false);


            CurrentShopUIs[shopType].shopContainer.SetActive(true);


            ShopIDAndBuyableItems? shopIDAndShopType = null;

            //Loop through all the keys and set the slots associated with that shop to true
            foreach (var shop in shopItems.Keys)
            {
                if (shop.shopID == shopID && shop.shopType == shopType)
                {
                    shopIDAndShopType = shop;
                }
            }

            //If the options don't exist
            if (!shopIDAndShopType.HasValue)
            {
                var newShopID = new ShopIDAndBuyableItems(shopType, shopID);

                shopItems.Add(newShopID, new List<SlotUI>());

                //3 attempts to fill a crafting station
                int attemptsRemaining = 5;

                var buyableItems = shopAccessing.GetItems();

                for (int i = 0; i < buyableItems.Count; i++)
                {
                    var buyableItem = buyableItems[i];


                    //Create a base UI Element
                    var buyableSlot = Instantiate(CurrentShopUIs[shopType].baseSlotUIPrefab,
                        CurrentShopUIs[shopType].optionsContainer);

                    var uiElements = BuildUIElements(buyableItem, CurrentShopUIs[shopType]);

                    foreach (var uiElement in uiElements)
                    {
                        buyableSlot.AddUIElements(uiElement.Key, uiElement.Value);
                    }

                    shopItems[newShopID].Add(buyableSlot);
                }
            }
            //Turn them all on
            else
            {
                foreach (var item in shopItems[shopIDAndShopType.Value])
                    item.gameObject.SetActive(true);
            }
        }

        //Build the UI Elements required
        private Dictionary<string, UIElement> BuildUIElements(BuyableItem buyableItem, ShopFeelAndLook look)
        {
            _cacheBuildUIElements ??= new Dictionary<string, UIElement>();

            _cacheBuildUIElements.Clear();


            if (buyableItem.icon)
            {
                _cacheBuildUIElements.Add("icon", look.slotUIFactory.Create(buyableItem.icon));
            }


            //Create a text element for the item name
            if (buyableItem.name != "")
            {
                _cacheBuildUIElements.Add("name", look.slotUIFactory.Create(buyableItem.name));
            }

            //Create amount text
            if (buyableItem.price != 0)
                _cacheBuildUIElements.Add("price", look.slotUIFactory.Create(buyableItem.price.ToString()));


            if (buyableItem.description != null)
                _cacheBuildUIElements.Add("description", look.slotUIFactory.Create(buyableItem.description));


            return _cacheBuildUIElements;
        }

        public void ServiceAdded()
        {
        }

        public void RemoveService()
        {
        }

        public void OnLocate()
        {
        }
    }


    [Serializable]
    public class ShopFeelAndLook
    {
        public GameObject shopContainer;
        public Transform optionsContainer;
        public SlotUI baseSlotUIPrefab;
        public UIElementFactory slotUIFactory;
    }

    public struct ShopIDAndBuyableItems
    {
        public uint shopID;
        public Shop.ShopType shopType;

        public ShopIDAndBuyableItems(Shop.ShopType shopType, uint shopID)
        {
            this.shopType = shopType;
            this.shopID = shopID;
        }
    }
}