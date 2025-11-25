using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SceneContainer
{

    /// <summary>
    /// Handles the state of the scene this is attached to.
    /// </summary>
    public class SceneRunner : MonoBehaviour
    {
        [Header("Root Object")] [SerializeField]
        private GameObject assetRootObject;


        [Header("On Entire Scene Load")] [SerializeField]
        private UnityEvent onOverallSceneLoaded;

        
        [Header("On Scene Turned On")]
        [SerializeField]private UnityEvent onSceneTurnedOn;

        
        [Header("On Scene Turned Off")]
        [SerializeField]private UnityEvent onSceneTurnedOff;
        

        [Header("States"), Tooltip("Scenes can have multiple states. States can be which objects load")]
        [SerializeField]
        private List<State> states;

        public bool IsActive;


        private void OnValidate()
        {
            if (!assetRootObject)
            {
                assetRootObject = transform.GetChild(0).gameObject;
            }
        }


        public void OnSceneLoaded()
        {
            Debug.Log("Scene loaded Event");

            onOverallSceneLoaded?.Invoke();
        }
        

        public void OnSceneLoaded(int state)
        {
            onOverallSceneLoaded?.Invoke();

            LoadState(state);
        }

        public void OnSceneLoaded(string state)
        {
            onOverallSceneLoaded?.Invoke();
            
            LoadState(state);
        }

        public void LoadState(string stateName)
        {
            var state = states.Find(x => x.stateName == stateName);
            
            state.StateRootObject.gameObject.SetActive(true);
            
            state.eventOnStateLoad?.Invoke();
            
        }

        public void LoadState(int stateIndex)
        {
            var state = states[stateIndex];
            
            state.StateRootObject.gameObject.SetActive(true);
            state.eventOnStateLoad?.Invoke();
        }



        public void TurnOnScene()
        {
            
            IsActive = true;
            assetRootObject.SetActive(true);
            StartCoroutine(WaitForFinishTurnOn());
        }


        public void TurnOffScene()
        {
            IsActive = false;
            
            onSceneTurnedOff?.Invoke();
            
            assetRootObject.SetActive(false);
        }



        IEnumerator WaitForFinishTurnOn()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            onSceneTurnedOn?.Invoke();
        }


        [Serializable]
        public struct State
        {
            public string stateName;
            public Transform StateRootObject;
            public UnityEvent eventOnStateLoad;
        }

    }
}
