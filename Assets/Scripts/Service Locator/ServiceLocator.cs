using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Utilities.Utilities;

namespace Service_Locator
{
    public class ServiceLocator : SingletonBehaviour<ServiceLocator>
    {
        private readonly Dictionary<string, object> _services = new();
        
        public void AddService(IService service, string nameOverride = "")
        {
            //Add's the service to the dictionary
            _services.Add((nameOverride != "") ? nameOverride : service.GetType().Name, service);

            service.ServiceAdded();
        }


        /// <summary>
        /// Locates a service by name.
        /// Highly recommended to use Locate<TService>() instead.
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IService Locate(string name)
        {
            return (IService)_services[name];
        }

        /// <summary>
        /// Locates a service by type.
        /// </summary>
        /// <param name="service"></param>
        /// <typeparam name="TService"></typeparam>
        /// TODO:: Async/Await
        public bool Locate<TService>(out TService service) where TService : Object
        {
            //Looks for the service in the dictionary
            var foundService = _services.Values.FirstOrDefault(i => i.GetType() == typeof(TService));

            //Casts the service to the type we want
            service = (TService)foundService;

            //If the service is an IService, call OnLocate()
            if (service is IService service1)
            {
                service1.OnLocate();
                return true;
            }
            
            
            //If we made it this far, find the object and attach LocatedService to it
            var foundObject = FindAnyObjectByType<TService>();
            
            if (foundObject != null)
            {
                var objService = foundObject.AddComponent<LocatedService>();
                service = foundObject;
                AddService(objService, foundObject.name);
                return true;
            }


            Debug.LogError("Couldn't find or create a service of type: " + typeof(TService).Name + "");
            return false;
        }
    }
}