using Interfaces;
using UnityEngine;

namespace Tools
{
    public class Tool : MonoBehaviour,IPlayerUseable
    {
        public virtual void OnLeftClick()
        {
     
        }

        public virtual void OnRightClick()
        {
        }
    }
}