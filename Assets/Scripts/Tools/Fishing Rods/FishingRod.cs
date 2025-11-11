using System;
using System.Collections;
using System.Collections.Generic;
using Alchemy.Inspector;
using Fishing;
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
        [SerializeField] private float chargeRate;
        
        //Change this to a state
        private bool _castedOut;



        [Title("Bobber")] 
        [SerializeField] private Bobber bobber;
        [SerializeField] private float bobberTravelTime;

        [Title("Fishing Start Point")]
        [SerializeField]private Transform startPoint;


        private Vector3 _endPoint;

        [Title("Cast Out Threshold")]
        [SerializeField]private float castOutThreshold;
        

        private bool _charging;
        private float _chargeAmount;
        
        [Title("Debug Values")]
        [SerializeField]private bool debug;

        
        

        private void Start()
        {
        }


        public override void OnLeftClick(InputAction.CallbackContext context)
        {
            bobber.transform.parent = startPoint;
            bobber.transform.position = Vector3.zero;
            _chargeAmount = 0;
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

            bobber.transform.position = startPoint.position;
            bobber.transform.parent = null;
            _charging = false;
            
            var playerCamera = StaticUtilities.GetMainCamera();

            Debug.Log("Casting Out");

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out var hit,
                    Mathf.Infinity))
            {
                Debug.Log(hit.transform.name);
                
               
                //Get the destination based on charge
                _endPoint = Vector3.Lerp(startPoint.position, hit.point, chargeAmount);
    
                //SEt the y of the end point to the y of the hit point
                _endPoint.y = hit.point.y;
                
                StartCoroutine(CastInCoroutine());
            }

            _castedOut = true;
        }


        
        //TODO: This can probably not be done in a coroutine, but for now it's fine
        
        private WaitForEndOfFrame cachedForEndOfFrame = new WaitForEndOfFrame();
        IEnumerator CastInCoroutine()
        {
            float timer = 0;
            
            while (timer < bobberTravelTime)
            {
                yield return cachedForEndOfFrame;
                bobber.transform.position = Vector3.Lerp(bobber.transform.position, _endPoint, timer / bobberTravelTime);
                timer += Time.deltaTime;
            }
        }

        private void CastIn()
        {
            Debug.Log("Cast In");
        }

        void OnDrawGizmosSelected()
        {
            if (!debug) return;
            
            
            Gizmos.color = Color.burlywood;
            Gizmos.DrawSphere(_endPoint, 0.15f);
        }
    }
}