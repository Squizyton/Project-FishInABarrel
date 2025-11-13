using System;
using System.Globalization;
using Alchemy.Inspector;
using Service_Locator;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour, IService
    {
        [Title("Interact ui")] public GameObject interactUI;

        [Title("Fishing UI")] [SerializeField] private GameObject fishingUI;


        [Title("Cash Value")] [SerializeField] private TextMeshProUGUI cashValue;

        public void Start()
        {
            //Add this as a service
            ServiceLocator.Instance.AddService(this);


            GameManager.Instance.inventory.OnMoneyChange += SetCashValue;
        }


        public void InteractStatus(bool status)
        {
            interactUI.SetActive(status);
        }


        public void FishingStatus(bool status)
        {
            fishingUI.SetActive(status);
        }

        public void SetCashValue(float value)
        {
            cashValue.text = value.ToString(CultureInfo.InvariantCulture);
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