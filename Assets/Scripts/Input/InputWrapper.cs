using Player;
using UnityEngine.InputSystem;
using Utilities.Utilities;

namespace Input
{
    public class InputWrapper : SingletonBehaviour<InputWrapper>
    {
        private PlayerControls _controls;


        public InputAction performMovement;
        public InputAction performJump;
        public InputAction performMouseMovement;
        public InputAction performLeftClick;
        public InputAction performInteract;
        
        public override void Awake()
        {
            base.Awake();

            _controls = new PlayerControls();

            performMovement = _controls.Player.Movement;
            performMouseMovement = _controls.Player.MouseLook;
            performLeftClick = _controls.Player.LeftClick;
            performInteract = _controls.Player.Interact;

            _controls.Enable();
        }
    }
}