using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using Fishing;
using Service_Locator;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.Utilities;
using Verlet_Intergration;

namespace Tools.Fishing_Rods
{
    public class FishingRod : Tool
    {
        [Title("Stats")] [SerializeField] private float maxDepth;
        public float MaxDepth => maxDepth;
        [SerializeField] private float speed;
        public float Speed => speed;
        [SerializeField] private float maxFish;
        public float MaxFish => maxFish;
        [SerializeField] private float chargeRate;

        //Change this to a state
        private bool _castedOut;


        [Title("Bobber")] [SerializeField] private Bobber bobber;
        public Bobber Bobber => bobber;
        [SerializeField] private float bobberTravelTime;

        [Title("Fishing Start Point")] [SerializeField]
        private Transform startPoint;


        private Vector3 _endPoint;

        [Title("Cast Out Threshold")] [SerializeField]
        private float castOutThreshold;


        private bool _charging;
        private float _chargeAmount;


        [Title("Procedural Animation")] [SerializeField]
        private Chaikin curve;

        [SerializeField] private LineRenderer fishingLinePrefab;
        [SerializeField] private VerletIntergration verletIntergration;
        private LineRenderer fishingLine;

        
        [Title("Debug Values")] [SerializeField]
        private bool debug;

        [SerializeField] private bool debugFishingLine;
        [SerializeField] private bool debugLinePhysics;

        [ShowIf("debugLinePhysics"), SerializeField]
        private int howManyPoints;
        private void Start()
        {
            fishingLine = Instantiate(fishingLinePrefab);
            
            if (debugLinePhysics)
            {
                //Create 
                fishingLine.positionCount = howManyPoints;
                
                Vector3[] linePoints = new Vector3[fishingLine.positionCount];
                
                
                fishingLine.GetPositions(linePoints);

                for (int i = 0; i < linePoints.Length; i++)
                {
                    linePoints[i] = Vector3.Lerp(startPoint.transform.position, bobber.transform.position,
                        i / (howManyPoints - 1f));
                }
                
                linePoints[0] = startPoint.transform.position;
                linePoints[howManyPoints - 1] = bobber.transform.position;
                
                fishingLine.SetPositions(linePoints);
                
                verletIntergration.SetupPoints(linePoints.ToList(),new []{0,howManyPoints-1});
            }
        }


        public override void OnLeftClick(InputAction.CallbackContext context)
        {
            bobber.transform.parent = startPoint;
            bobber.transform.position = startPoint.position;
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



            if (debugLinePhysics)
            {
                // fishingLine.SetPosition(0, startPoint.position);
                // fishingLine.SetPosition(fishingLine.positionCount-1, bobber.transform.position);


                // Vector3[] linePoints = new Vector3[fishingLine.positionCount];
                // fishingLine.GetPositions(linePoints);
                
                //verletIntergration.UpdateEachPointCurrentPosition(linePoints.ToList());
                
                
                
                verletIntergration.UpdatePointIndex(0,startPoint.position);
                verletIntergration.UpdatePointIndex(howManyPoints-1,bobber.transform.position);
                
                fishingLine.SetPositions(verletIntergration.ReturnRawPoints().ToArray());
                
                
            }

        }

        public override void OnLeftClickUp(InputAction.CallbackContext context)
        {
            if (_chargeAmount > castOutThreshold)
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
                bobber.transform.position =
                    Vector3.Lerp(bobber.transform.position, _endPoint, timer / bobberTravelTime);
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