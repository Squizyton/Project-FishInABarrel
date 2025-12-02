using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;

namespace Shops
{
    public class SlotUI : MonoBehaviour
    {
        
        [Title("Shop Items")] 
        private Dictionary<string, UIElement> uiElements;

        private Transform _container;


        public void AddUIElements(string elementName, UIElement element)
        {
            uiElements ??= new Dictionary<string, UIElement>();
            
            uiElements.Add(elementName, element);
        }

        public UIElement GetUIElement(string elementName)
        {
            return uiElements[elementName];
        }
    }
}