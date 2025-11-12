using System;
using Alchemy.Inspector;
using Service_Locator;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour,IService
    {
        [Title("Interact ui")]
        public GameObject interactUI;
        
        [Title("Fishing UI")]
        [SerializeField] private GameObject fishingUI;
        
        
        public void Start()
        {
            //Add this as a service
            ServiceLocator.Instance.AddService(this);
        }





        public void InteractStatus(bool status)
        {
            interactUI.SetActive(status);            
        }



        public void FishingStatus(bool status)
        {
            fishingUI.SetActive(status);       
        }



        public void ServiceAdded()
        {
            Debug.Log("UI Manager Added to Service Locator");
        }

        public void RemoveService()
        {
        }

        public void OnLocate()
        {
        }
    }
}