using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Alchemy.Serialization;
using Shops.UIElements;
using UnityEngine;

namespace Shops
{
    [AlchemySerialize]
    public partial class SlotUI : BaseUIElement
    {
        [Title("UI Anchor Points")] [AlchemySerializeField, NonSerialized]
        private Dictionary<string, Transform> anchorPoints;

        [Title("Shop Items")] private Dictionary<string, UIElement> uiElements;

        private Transform _container;


        public void AddUIElements(string elementName, UIElement element)
        {
            uiElements ??= new Dictionary<string, UIElement>();


            if (anchorPoints.TryGetValue(elementName, out var point))
            {
                element.transform.SetParent(point);
                element.transform.localPosition = Vector3.zero;
                element.transform.localScale = Vector3.one;
                uiElements.Add(elementName, element);
            }
            else
            {
                //Toss the ui element
                Debug.LogWarning($"{elementName} doesn't exist as anchor point");
                Destroy(element.gameObject);
            }
            
            
        }

        public UIElement GetUIElement(string elementName)
        {
            return uiElements[elementName];
        }
    }
}