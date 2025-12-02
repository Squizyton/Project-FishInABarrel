using TMPro;
using UnityEngine;

namespace Shops.UIElements
{
    public class TextUIElement : UIElement
    {
        private TextMeshProUGUI _text;


        private void OnStart()
        {
            
        }

        public override void OnInitialize<T>(T data)
        {
            if(data is string text)
                _text.text = text;
        }
    }
}