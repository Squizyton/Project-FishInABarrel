using Input;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools
{
    public class Tool : MonoBehaviour, IPlayerUseable
    {
        public void OnSelect()
        {
            InputWrapper.Instance.performLeftClick.performed += OnLeftClick;
            InputWrapper.Instance.performLeftClick.canceled += OnLeftClickUp;
            InputWrapper.Instance.performRightClick.performed += OnRightClick;
            InputWrapper.Instance.performRightClick.canceled += OnRightClickUp;
        }

        public void OnDeselect()
        {
            InputWrapper.Instance.performLeftClick.performed -= OnLeftClick;
            InputWrapper.Instance.performLeftClick.canceled -= OnLeftClickUp;
            InputWrapper.Instance.performRightClick.performed -= OnRightClick;
            InputWrapper.Instance.performRightClick.canceled -= OnRightClickUp;
        }

        public virtual void OnLeftClick(InputAction.CallbackContext context)
        {
        }

        public virtual void OnLeftClickUp(InputAction.CallbackContext context)
        {
        }

        public virtual void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public virtual void OnRightClickUp(InputAction.CallbackContext context)
        {
        }
    }
}