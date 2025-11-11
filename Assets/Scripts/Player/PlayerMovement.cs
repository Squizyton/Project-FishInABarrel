    using System;
    using Alchemy.Inspector;
    using Input;
    using Service_Locator;
    using UnityEngine;
    using UnityEngine.InputSystem;

    namespace Player
    {
        public class PlayerMovement : MonoBehaviour, IService
        {
            
            [Title("Movement Variables")] [SerializeField]
            private float moveSpeed = 5f;
            [SerializeField] private float sprintMultiplier = 1.5f;
            [SerializeField] private float jumpForce = 5f;
            private Rigidbody _rb;


            public enum State
            {
                Idle,
                Walking,
                Fishing,
            }

            public State CurrentState { get; private set; }

            public event Action<int> OnStateChange;
            
            void Start()
            {
                       
                InputWrapper.Instance.performJump.performed += HandleJump;
                _rb = GetComponent<Rigidbody>();
                
                ServiceLocator.Instance.AddService(this);
            }


            void FixedUpdate()
            {
                HandleMovement();
            }

            void HandleMovement()
            {
                float moveX = InputWrapper.Instance.performMovement.ReadValue<Vector2>().x;
                float moveZ = InputWrapper.Instance.performMovement.ReadValue<Vector2>().y;

                Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
                //float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;

                Vector3 velocity = new Vector3(moveDirection.x * moveSpeed, _rb.linearVelocity.y, moveDirection.z * moveSpeed);
                _rb.linearVelocity = velocity;
            }

            void HandleJump(InputAction.CallbackContext ctx)
            {
                if (!IsGrounded()) return;

                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            bool IsGrounded()
            {
                return Physics.Raycast(transform.position, Vector3.down, 1.1f);
            }

            public void ChangeState(State state)
            {
                CurrentState = state;
                OnStateChange?.Invoke((int)state);
            }


            public void ServiceAdded()
            {
                Debug.Log("Player Movement Added to Service Locator");
            }

            public void RemoveService()
            {
             
            }

            public void OnLocate()
            {
            }
        }
    }
