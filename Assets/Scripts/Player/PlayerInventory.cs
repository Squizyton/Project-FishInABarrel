using System;
using System.Collections.Generic;
using Alchemy.Inspector;
using Input;
using Interfaces;
using Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public const int MaxTools = 4;

        [SerializeField] private Transform toolHolder;
        [SerializeField] private PlayerInteraction playerInteraction;


        private float _money;
        public float CurrentMoney => _money;

        public int _currentToolIndex = 0;

        public event Action<float> OnMoneyChange;

        private EquippedTool[] _equippedTools = new EquippedTool[MaxTools];


        [Title("Starting Tools", "These tools will be added to the inventory when the game starts")] [SerializeField]
        private List<Tool> startingTools;


        private void Start()
        {
            for (int i = 0; i < MaxTools; i++)
            {
                _equippedTools[i] = new EquippedTool { IsEmpty = true };
            }


            if (startingTools.Count > 0)
            {
                for (int i = 0; i < startingTools.Count; i++)
                    AddToolToInventory(startingTools[i], (i == 0));
            }


            InputWrapper.Instance.performSwitchTool.performed += EquipTool;
        }


        public void AddToolToInventory(Tool newTool, bool autoEquip = false)
        {
            for (int i = 0; i < MaxTools; i++)
            {
                if (!_equippedTools[i].IsEmpty) continue;
                
                Debug.Log($"Adding tool {newTool.name} to inventory");
                
                
                
                
                
                Tool go = Instantiate(newTool, toolHolder);

                go.transform.SetParent(toolHolder);

                go.transform.localPosition = newTool.transform.localPosition;
                go.transform.localRotation = newTool.transform.localRotation;
                go.gameObject.SetActive(false);

                _equippedTools[i] = new EquippedTool { ToolReference = go };

                
                if (autoEquip)
                {
                    Debug.Log($"Equipping tool {newTool.name}");
                    playerInteraction.FeedUseableItem(go);
                }

                return;
            }
        }


        public void EquipTool(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<float>();

            int previousIndex = _currentToolIndex;

            _currentToolIndex = (_currentToolIndex + (int)value) % _equippedTools.Length;

            
            if(_currentToolIndex < 0) _currentToolIndex = _equippedTools.Length - 1;
            
            if (!_equippedTools[_currentToolIndex].IsEmpty)
            {
                playerInteraction.FeedUseableItem(_equippedTools[_currentToolIndex].ToolReference);
            }
            
            
        }

        public void EquipTool()
        {
        }


        /// <summary>
        /// Adds cash with a multiplier.
        /// </summary>
        /// <param name="baseAmount"></param>
        /// <param name="mutliplyer"></param>
        public void AddCash(float baseAmount, float mutliplyer)
        {
            Debug.Log($"Adding {baseAmount} with multiplier {mutliplyer}");
            float amountToAdd = baseAmount + (baseAmount * mutliplyer);
            _money += amountToAdd;
            OnMoneyChange?.Invoke(_money);
        }

        /// <summary>
        /// Can Add or Remove (HAS NO MULTIPLIER)
        /// </summary>
        /// <param name="amount"></param>
        public void ManipulateMoney(float amount)
        {
            _money += amount;
            OnMoneyChange?.Invoke(_money);
        }


        private struct EquippedTool
        {
            public bool IsEmpty;
            public Tool ToolReference;
        }
    }
}