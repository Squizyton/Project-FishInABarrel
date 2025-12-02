using System.Collections.Generic;
using UnityEngine;

namespace Shops.UIElements
{
    public class BaseUIElement : UIElement
    {
        [SerializeField]private Dictionary<string, Transform> anchorPoints;
    }
}