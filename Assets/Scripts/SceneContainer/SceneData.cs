using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneContainer
{
    [CreateAssetMenu(fileName = "Scene Data", menuName = "Scene Data", order = 0)]
    public class SceneData : ScriptableObject
    {
        public string sceneName => name;

        public Sprite loadingSprite;
        
        
        
        [Header("Main Scene"),Tooltip("The main scene that gets loaded")]
        public SceneReference mainScene;


        [Header("Sub scenes"),Tooltip("The sub scenes that gets loaded but then get turned off")] 
        public List<SceneReference> subScenes;

    }
}