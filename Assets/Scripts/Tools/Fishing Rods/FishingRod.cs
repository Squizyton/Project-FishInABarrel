using System;
using Alchemy.Inspector;
using Service_Locator;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.Utilities;

namespace Tools.Fishing_Rods
{
    public class FishingRod : Tool
    {
        [Title("Stats")]
        [SerializeField] private float maxDepth;
        [SerializeField] private float speed;
        [SerializeField] private float maxFish;

        //Change this to a state
        private bool _castedOut;



        [Title("Fishing Start Point")]
        [SerializeField]private Transform startPoint;


        private Vector3 _endPoint;

        [Title("Cast Out Threshold")]
        [SerializeField]private float castOutThreshold;
        

        private bool _charging;
        private float _chargeAmount;
        private bool _debug;


        private void Start()
        {
            var test = ServiceLocator.Instance.Locate<GameManager>(out var gameManager);
        }


        public override void OnLeftClick(InputAction.CallbackContext context)
        {
            _charging = true;
        }


        private void Update()
        {
            if (_charging)
            {
                _chargeAmount = Mathf.Clamp(_chargeAmount + Time.deltaTime, 0, 1);
                Debug.Log(_chargeAmount);   
            }
        }

        public override void OnLeftClickUp(InputAction.CallbackContext context)
        {
            if(_chargeAmount > castOutThreshold)
                CastOut(_chargeAmount);
        }

        private void CastOut(float chargeAmount)
        {
            
            var playerCamera = StaticUtilities.GetMainCamera();


            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out var hit,
                    Mathf.Infinity))
            {
               
                //Get the destination based on charge
                _endPoint = Vector3.Lerp(startPoint.position, hit.point, chargeAmount);
    
                //SEt the y of the end point to the y of the hit point
                _endPoint.y = hit.point.y;


                
                

            }

            _castedOut = true;
        }

        private void CastIn()
        {
            Debug.Log("Cast In");
        }

        void OnDrawGizmosSelected()
        {
            if (!_debug) return;
            
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPoint.position, _endPoint);
        }
    }
}