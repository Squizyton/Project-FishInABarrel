using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


//TODO:: Decouple this from Game Itself
//I Want to be able to Keep things seperate
namespace SceneContainer
{
    /// <summary>
    /// TODO:: Joel's note: This game might not be super demanding, in which case Refactor to just load everything, however wanted to future proof for now
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [Header("Scene Controller settings")] [SerializeField]
        private int maxNumberOfScenesLoadedAtATime = 3;

        private Queue<SceneData> _currentlyLoadedScenes = new Queue<SceneData>();


        private Dictionary<string, SceneData> _sceneDictionary = new Dictionary<string, SceneData>();

        private SceneData _currentActiveScene;
        private Dictionary<SceneData, SceneRunner> _sceneRunners = new Dictionary<SceneData, SceneRunner>();


        [FormerlySerializedAs("startScene")] [Header("Debug Variables")]
        public SceneData debugStartScene;

        public Action onSceneChange;

        public void Awake()
        {
            //Load all the SceneData objects into Dictionary
            var sceneDataObjects = Resources.LoadAll<SceneData>("Scriptable Objects/Scenes");
            foreach (var sceneDataObject in sceneDataObjects)
            {
                _sceneDictionary.Add(sceneDataObject.name, sceneDataObject);
            }
        }


        private SceneData ReturnData(string sceneName)
        {
            if (_sceneDictionary.TryGetValue(sceneName, out var data))
            {
                return data;
            }
            else
            {
                Debug.LogError($"Scene {sceneName} not found in scene dictionary");
                return null;
            }
        }
        
        public void LoadScene(string sceneName)
        {
            var data = ReturnData(sceneName);
            //Cache the current Active scene
            SceneData previousActiveScene = _currentActiveScene;
            
            
            onSceneChange?.Invoke();
             
            //First check to see if the scene is already loaded
            if (_currentlyLoadedScenes.Contains(data))
            {
                // Grab the Scene
                Scene scene = SceneManager.GetSceneByName(sceneName);

                SceneManager.SetActiveScene(scene);

                //Grab the SceneRunner
                if (_sceneRunners.TryGetValue(data, out var runner))
                {
                    runner.TurnOnScene();

                    _currentActiveScene = data;
                    
                    //Turn off the previous scene
                    if (previousActiveScene)
                    {
                        if (_sceneRunners.TryGetValue(previousActiveScene, out var previousRunner))
                        {
                            previousRunner.TurnOffScene();
                        }
                    }

                    return;
                }
            }

            //Check to see if the there are too many scenes loaded
            if (_currentlyLoadedScenes.Count >= maxNumberOfScenesLoadedAtATime)
            {
                //pop the oldest scene off the queue that is not active
                var sceneData = _currentlyLoadedScenes.Dequeue();

                //Check to make sure its not active
                if (!_sceneRunners[sceneData].IsActive)
                {
                    //Unload the main scene
                    SceneManager.UnloadSceneAsync(sceneData.mainScene);

                    //unload the sub scenes
                    foreach (var subScene in sceneData.subScenes)
                    {
                        SceneManager.UnloadSceneAsync(subScene);
                    }
                }

                //Remove the runner from the dictionary
                _sceneRunners.Remove(sceneData);
            }
            
            //Load the main scene
            SceneManager.LoadSceneAsync(data.mainScene, LoadSceneMode.Additive)!.completed += _ =>
            {
                //Set it as the active scene
                Scene loadedScene = SceneManager.GetSceneByName(sceneName);

                SceneManager.SetActiveScene(loadedScene);

                var sceneRunner = loadedScene.GetRootGameObjects()[0].GetComponent<SceneRunner>();

                if (sceneRunner)
                {
                    sceneRunner.TurnOnScene();

                    sceneRunner.OnSceneLoaded();
                    //Add the scene to the active scene dictionary
                    _sceneRunners.Add(data, sceneRunner);


                    _currentActiveScene = data;

                    //Turn off the previous scene
                    if (previousActiveScene)
                    {
                        if (_sceneRunners.TryGetValue(previousActiveScene, out var previousRunner))
                        {
                            previousRunner.TurnOffScene();
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Scene {sceneName} not found in scene dictionary or scene runner not found");
                    return;
                }

                //Start loading the sub scenes
                SceneManager.sceneLoaded += OnSubSceneLoaded;
                
                //load the sub scenes
                foreach (var subScene in data.subScenes)
                {
                    SceneManager.LoadSceneAsync(subScene, LoadSceneMode.Additive);
                }


                //Add the scene to the queue
                _currentlyLoadedScenes.Enqueue(data);
            };

            SceneManager.sceneLoaded -= OnSubSceneLoaded;
        }

        public SceneData GetCurrentActiveScene()
        {
            return _currentActiveScene;
        }


        public void OnSubSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            scene.GetRootGameObjects()[0].SetActive(false);
        }

        public void LoadSubScene(SceneReference sceneRef)
        {
            //Developer's Note:: For some odd reason GetScene by name refuses to work and returns an Invalid reference, SceneByPath works.
            var scene = SceneManager.GetSceneByPath(sceneRef.ScenePath);
          
            scene.GetRootGameObjects()[0].SetActive(true);
        }


        public void UnloadSubScene(SceneReference sceneRef)
        {
            var scene = SceneManager.GetSceneByPath(sceneRef.ScenePath);
            scene.GetRootGameObjects()[0].SetActive(false);
        }

        public SceneData GetData(string sceneName)
        {
            return _sceneDictionary[sceneName];
        }
    }
}