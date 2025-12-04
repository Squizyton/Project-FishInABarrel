using TMPro;
using UnityEngine;

namespace Shops.UIElements
{
    public class TextUIElement : UIElement
    {
        private TextMeshProUGUI _text;


        private void Awake()
        {
            _text = gameObject.AddComponent<TextMeshProUGUI>();
        }

        public override void OnInitialize<T>(T data)
        {
            if (data is string text)
            {
                Debug.Log(text);
                _text.text = text;
            }
        }
    }
}