using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shops.UIElements
{
    public class ImageUIElement : UIElement
    {
        private Image _image;
        
        private void Awake()
        {
            _image = gameObject.AddComponent<Image>();
        }


        public override void OnInitialize<TSprite>(TSprite data)
        {
            if (data is Sprite sprite)
            {
                _image.sprite = sprite;
            }
            else
            {
                Debug.LogError($"Invalid Sprite Type, expected {nameof(Sprite)} but got {data.GetType().Name}");
            }
        }
    }
}