using System;
using System.Collections.Generic;
using Fishing;
using Input;
using Tools.Fishing_Rods;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utilities.Utilities;
using Random = UnityEngine.Random;

public class FishingMiniGame : MonoBehaviour
{
    private FishingRod _currentFishingRod;

    [SerializeField] private Transform startingPoint;
    [SerializeField] private MinigameBobber gameBobber;

    [FormerlySerializedAs("_fishContainer")] [SerializeField]
    private Transform fishContainer;


    [SerializeField] private FollowObject followObject;


    private List<SpawnedFish> _spawnedFish;
    private List<CaughtFish> _caughtFish;

    private int _startingFishCount = 20;

    //-1 means down, 1 means up
    private float _currentBobberDirection = -1;
    private bool _goingUp;
    private bool _gameOver;

    [SerializeField] private GameObject fishPrefab;
    private Vector2 _movement;


    private FishingManager _fishingManager;


    private void Awake()
    {
        _caughtFish = new List<CaughtFish>();
        _spawnedFish = new List<SpawnedFish>();
        _fishingManager = FindAnyObjectByType<FishingManager>();
    }


    public void BobberMovement(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>();
    }

    public void StartGame(FishingRod fishingRod, ref List<FishData> fishThatCanSpawn)
    {
        _currentFishingRod = fishingRod;
        InputWrapper.Instance.performBobberMovement.performed += BobberMovement;
        InputWrapper.Instance.performBobberMovement.canceled += BobberMovement;
        _caughtFish.Clear();
        _spawnedFish.Clear();


        for (int i = 0; i < _startingFishCount; i++)
        {
            var startingPosition = new Vector3(Random.Range(-3, 3), Random.Range(-_currentFishingRod.MaxDepth, 0f),
                startingPoint.position.z);


            FishData pickedData = fishThatCanSpawn[Random.Range(0, fishThatCanSpawn.Count)];
            var spawnedFish = Instantiate(pickedData.inWaterFishPrefab, startingPosition, Quaternion.identity);


            spawnedFish.transform.SetParent(fishContainer.transform);
            _spawnedFish.Add(new SpawnedFish() { fishData = pickedData, fishReference = spawnedFish });
        }

        gameBobber.transform.position = startingPoint.position;
        _currentBobberDirection = -1;
        _gameOver = false;
    }


    private void Update()
    {
        if (_gameOver) return;


        if ((Mathf.Abs(gameBobber.transform.position.y) >= _currentFishingRod.MaxDepth && !_goingUp) ||
            _caughtFish.Count > _currentFishingRod.MaxFish)

        {
            _currentBobberDirection = 1;
            _goingUp = true;
        }

        if (_goingUp)
        {
            //Debug.Log(startingPoint.transform.position.y.OneValueDistance(gameBobber.transform.position.y));
            if (startingPoint.transform.position.y.OneValueDistance(gameBobber.transform.position.y) < 0.1f)
            {
                EndGame();
            }
        }
    }


    private void FixedUpdate()
    {
        if (_gameOver) return;


        gameBobber.transform.Translate(_movement.x * Time.deltaTime * _currentFishingRod.Speed, 0, _movement.y);


        //Clamp the bobber
        if (gameBobber.transform.position.x > 3)
            gameBobber.transform.position =
                new Vector3(3, gameBobber.transform.position.y, gameBobber.transform.position.z);
        if (gameBobber.transform.position.x < -3)
            gameBobber.transform.position =
                new Vector3(-3, gameBobber.transform.position.y, gameBobber.transform.position.z);


        Vector3 bobberPosition = gameBobber.transform.position;
        bobberPosition.y += _currentBobberDirection * Time.deltaTime * _currentFishingRod.Speed;
        gameBobber.transform.position = bobberPosition;
    }


    public bool OnFishCaught(Transform caughtFish)
    {
        if (_caughtFish.Count < _currentFishingRod.MaxFish)
        {
            //TODO LINQ Clean this up
            var fishStruct = _spawnedFish.Find(x => x.fishReference == caughtFish.gameObject);

            _caughtFish.Add(new CaughtFish() { fishData = fishStruct.fishData });

            //Destroy(fishStruct.fishReference);

            //TODO:: Optimize
            _spawnedFish.Remove(fishStruct);

            return true;
        }

        return false;
    }


    private void EndGame()
    {
        InputWrapper.Instance.performBobberMovement.performed -= BobberMovement;
        _gameOver = true;
        Debug.Log(_caughtFish.Count);
        _fishingManager.StopFishing(ref _caughtFish);
    }

    private struct SpawnedFish
    {
        public GameObject fishReference;
        public FishData fishData;
    }


    public struct CaughtFish
    {
        //Add FishData and stuff here
        public FishData fishData;
    }
}