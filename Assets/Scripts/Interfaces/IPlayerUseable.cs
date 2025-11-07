using UnityEngine.InputSystem;

namespace Interfaces
{
    public interface IPlayerUseable
    {

        void OnSelect();
        void OnDeselect();
        
        void OnLeftClick(InputAction.CallbackContext context);
        void OnLeftClickUp(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnRightClickUp(InputAction.CallbackContext context);
    }
}