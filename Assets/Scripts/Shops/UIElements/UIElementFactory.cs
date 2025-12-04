using Alchemy.Inspector;
using UnityEngine;

namespace Shops.UIElements
{
    [CreateAssetMenu(fileName = "UIElementFactory", menuName = "Shops/UI Element Factory")]
    public class UIElementFactory : ScriptableObject
    {
        [Title("UI Prefabs")]
        [SerializeField]private TextUIElement textUIElement;
        [SerializeField]private ImageUIElement imageUIElement;
        [SerializeField]private BaseUIElement baseUIElement;



        public UIElement Create(object data)
        {
            if (data is string textData)
            {
                if (textUIElement == null)
                {
                    Debug.LogError($"{name}'s TextUIElement is null.");
                    return null;
                }

                var instance  = Instantiate(textUIElement);
                instance.OnInitialize(textData);
                return instance;
            }

            if (data is Sprite spriteData)
            {
                if (imageUIElement == null)
                {
                    Debug.LogError($"{name}'s ImageUIElement is null.");
                    return null;
                }
                var instance = Instantiate(imageUIElement);
                instance.OnInitialize(spriteData);
                return instance;
            }

            Debug.LogError($"Unknown data type {data.GetType()}");
            return null;
        }

        /// <summary>
        /// Explicitly create a base element if needed.
        /// </summary>
        public BaseUIElement CreateBase(Transform parent)
        {
            return Instantiate(baseUIElement, parent);
        }
    

    }
}
