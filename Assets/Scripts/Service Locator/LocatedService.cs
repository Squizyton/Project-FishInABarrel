using UnityEngine;

namespace Service_Locator
{
    
    public class LocatedService :MonoBehaviour, IService
    {
        public void ServiceAdded()
        {
            Debug.LogWarning($"This Service: {transform.name} was added, by default this object is not considered a service");
        }

        public void RemoveService()
        {
            Debug.LogWarning("Non Defaulted Service Removed");
            
            Destroy(this);
        }

        public void OnLocate()
        {
        }
    }
}