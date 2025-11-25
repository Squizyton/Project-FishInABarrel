using System;
using System.Collections;
using System.Collections.Generic;
using Alchemy.Inspector;
using Fishing;
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

        private bool _castedOut;

        [Title("Bobber")] [SerializeField] private Bobber bobber;
        public Bobber Bobber => bobber;
        [SerializeField] private float bobberTravelTime;

        [Title("Fishing Start Point")] [SerializeField]
        private Transform startPoint;

        private Vector3 _endPoint;

        [Title("Cast Out Threshold")] [SerializeField]
        private float forwardForceAmount;

        [SerializeField] private float upForceAmount;
        [SerializeField] private float castOutThreshold;

        private bool _charging;
        private float _chargeAmount;

        [Title("Procedural Animation")] [SerializeField]
        private Chaikin curve;

        [SerializeField] private float distanceBetweenEachChaikinCheck;
        [SerializeField] private LineRenderer fishingLinePrefab;
        [SerializeField] private VerletIntergration verletIntergration;
        private LineRenderer fishingLine;

        [Title("Debug Values")] [SerializeField]
        private bool debug;
        [SerializeField] private bool debugFishingLine;
        [ShowIf("debugLinePhysics"), SerializeField]
        private int howManyPoints;

        private Coroutine verletCoroutine;
        private Vector3 _bobberOutPreviousPosition;
        private readonly WaitForEndOfFrame _cachedEndOfFrame = new WaitForEndOfFrame();

        // Buffer resized to 100 to start to avoid immediate resize
        private Vector3[] linePositionsBuffer = new Vector3[100]; 
        private List<Vector3> _smoothingCacheList = new List<Vector3>();

        private void Start()
        {
            fishingLine = Instantiate(fishingLinePrefab);
        }

        public override void OnLeftClick(InputAction.CallbackContext context)
        {
            bobber.rb.isKinematic = true;
            bobber.transform.SetParent(startPoint);
            bobber.transform.position = startPoint.position;
            bobber.castedOut = false;
            bobber.isBusy = false;
            
            
            verletIntergration.Clear();
            fishingLine.positionCount = 0;

            if (verletCoroutine != null)
                StopCoroutine(verletCoroutine);

            _chargeAmount = 0;
            _charging = true;
        }

        private void Update()
        {
            if (_charging)
            {
                _chargeAmount = Mathf.Clamp(_chargeAmount + Time.deltaTime, 0, 1);
                bobber.transform.position = startPoint.position;
            }
        }
        
        // FIX 2: You likely need to run the physics simulation here or in FixedUpdate
        private void FixedUpdate()
        {
            if (_castedOut)
            {
                // Assuming your verlet script has a Simulate method. 
                // If not, ensure the logic inside VerletIntegration runs automatically.
                // verletIntergration.Simulate(Time.fixedDeltaTime); 
            }
        }

        private void LateUpdate()
        {
            if (_castedOut)
            {
                // FIX 1: Ensure this returns a LIST (Ordered), not a HashSet
                var rawPoints = verletIntergration.ReturnRawPoints();
                int pointCount = rawPoints.Count;

                if (pointCount > 1)
                {
                    // Update ends to pin the rope
                    verletIntergration.UpdatePointIndex(0, startPoint.position);
                    verletIntergration.UpdatePointIndex(pointCount - 1, bobber.transform.position);

                    // Pass the list to the renderer
                    UpdateLineRenderer(rawPoints);
                }
            }
        }

        // FIX 1: Changed parameter from HashSet to List<Vector3> to preserve order
        private void UpdateLineRenderer(List<Vector3> rawPoints)
        {
            int count = rawPoints.Count;

            if (linePositionsBuffer.Length < count)
            {
                Array.Resize(ref linePositionsBuffer, count * 2);
            }

            // CopyTo works correctly on Lists/Arrays (Ordered)
            rawPoints.CopyTo(linePositionsBuffer);

            fishingLine.positionCount = count;
            fishingLine.SetPositions(linePositionsBuffer);
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

            _bobberOutPreviousPosition = bobber.transform.position;
            var playerCamera = StaticUtilities.GetMainCamera();

            bobber.rb.isKinematic = false;
            bobber.isBusy = false;
            bobber.rb.AddForce(playerCamera.transform.forward * (forwardForceAmount * chargeAmount), ForceMode.Impulse);
            bobber.castedOut = true; // Assumes this is a public field on Bobber
            
            bobber.rb.AddForce(Vector3.up * upForceAmount, ForceMode.Impulse);

            verletIntergration.UpdateGravityDirection(Vector3.up);

            // Setup initial points so we have something to render immediately
            InitializeVerletLine();

            verletCoroutine = StartCoroutine(UpdateVerletAfterBobberLanding());
            _castedOut = true;
        }

        private void InitializeVerletLine()
        {
             _smoothingCacheList.Clear();
             _smoothingCacheList.Add(startPoint.position);
             _smoothingCacheList.Add(bobber.transform.position);
             
             // Initial straight line setup
             var initialPoints = curve.ApplyChaikinSmoothing(ref _smoothingCacheList);
             verletIntergration.SetupPoints(initialPoints, new[] { 0, initialPoints.Count - 1 });
        }

        private IEnumerator UpdateVerletAfterBobberLanding()
        {
            yield return _cachedEndOfFrame;

            while (bobber.rb.linearVelocity.sqrMagnitude > 0.0001f)
            {
                float distSq = (startPoint.position - bobber.transform.position).sqrMagnitude;
                float threshSq = distanceBetweenEachChaikinCheck * distanceBetweenEachChaikinCheck;


                if (distSq > threshSq)
                {
                    _smoothingCacheList.Clear();
                    _smoothingCacheList.Add(startPoint.position);
                    _smoothingCacheList.Add(bobber.transform.position);
                    
                    var smoothedPoints = curve.ApplyChaikinSmoothing(ref _smoothingCacheList);

                    verletIntergration.SetupPoints(smoothedPoints, new[] { 0, smoothedPoints.Count - 1 });

                    fishingLine.positionCount = smoothedPoints.Count;
                }

                _bobberOutPreviousPosition = bobber.transform.position;
                yield return _cachedEndOfFrame;
            }

            // Once stopped, we switch gravity down and stop resetting the points
            // allowing the line to finally sag.
            verletIntergration.UpdateGravityDirection(Vector2.down);
        }
        [Button]
        public void SimulateFishing()
        {
            _castedOut = false;
            bobber.rb.isKinematic = true;
            bobber.isBusy = false;
            
            bobber.transform.SetParent(startPoint.transform);
            bobber.transform.position = startPoint.position;
            fishingLine.positionCount = 0;
            
            verletIntergration.Clear();
            CastOut(1);
        }

        public void StopFishing()
        {
            _castedOut = false;
            bobber.castedOut = false;
            bobber.isBusy = false;
            bobber.rb.isKinematic = true;
            bobber.transform.parent = startPoint.transform;
        }
    }
}