using System;
using Alchemy.Inspector;
using Service_Locator;
using Unity.VisualScripting;
using UnityEngine;
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

        private void Start()
        {
            var test = ServiceLocator.Instance.Locate<GameManager>(out var gameManager);
        }

        public override void OnLeftClick()
        {
            if (!_castedOut)
            {
                CastOut();
            }
            else
            {
                CastIn();
            }
        }


        private void CastOut()
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