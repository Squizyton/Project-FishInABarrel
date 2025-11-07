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
               
            }

            _castedOut = true;
        }

        private void CastIn()
        {
            Debug.Log("Cast In");
        }
    }
}