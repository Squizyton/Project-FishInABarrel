using System;
using Player;
using Service_Locator;
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
        public InputAction performRightClick;
        public InputAction performInteract;
        
        
        //Fishing
        public InputAction performBobberMovement;
        
        
        public override void Awake()
        {
            base.Awake();

            _controls = new PlayerControls();

            //Player
            performMovement = _controls.Player.Movement;
            performMouseMovement = _controls.Player.MouseLook;
            performLeftClick = _controls.Player.LeftClick;
            performInteract = _controls.Player.Interact;

            //Fishing
            performBobberMovement = _controls.Fishing.BobberMovement;
            
            
            _controls.Enable();
        }


        private void Start()
        {
            //Locate the service
            ServiceLocator.Instance.Locate(out PlayerMovement movement);
            movement.OnStateChange += OnStateChange;
        }

        public void OnStateChange(int state)
        {
            _controls.Player.Disable();
            _controls.Fishing.Disable();
            
            
            switch(state)
            {
                //Walking
                case 1:
                    _controls.Player.Enable();
                    break;
                //Fishing
                case 2:
                    _controls.Fishing.Enable();
                    break;
            }
        }
    }
}