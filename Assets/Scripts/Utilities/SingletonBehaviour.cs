using UnityEngine;

namespace Utilities.Utilities
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]private bool dontDestroyOnLoad = true;
        
        private static T instance;


        public static bool HasInstance;
        
        public static T Instance
        {
            get
            {
                if (instance) return instance;
                
                
                Debug.Log("Creating a new instance");
                
                var emptyObject = new GameObject
                {
                    name = typeof(T).Name
                };
                
                instance = emptyObject.AddComponent<T>();
                
                HasInstance = true;
                
                return instance;
            }
        }
    
            public virtual void Awake() {
            if (instance == null)
            {
                
                instance = this as T;

                if (!dontDestroyOnLoad) return;
                
                //Since destroy on load requires the object to be a root, remove the object from its parent
                if (transform.parent)
                {
                    transform.parent = null;
                }
                    
                HasInstance = true;
                
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                
                Debug.LogError($"You already have an instance of {typeof(T).Name}, Destroying {this.gameObject}");
                Destroy(this.gameObject);
            }
        }
        
        
        
        
        private void OnApplicationQuit()
        {
            Destroy(instance);
        }
    }
}
