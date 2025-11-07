using System;
using System.Collections.Generic;
using Abilities;
using Abilities.Abilities;
using Abilities.Effects.Factories;
using Abilities.Interfaces;
using Alchemy.Inspector;
using Guns;
using Input;
using Interfaces;
using Managers;
using Service_Locator;
using Tools;
using Tools.Fishing_Rods;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class PlayerInteraction : MonoBehaviour, IService
{
    [Title("Item Holder")] [SerializeField]
    private Transform itemHolder;


    [Title("Values")] [SerializeField] private LayerMask interactLayerMask;

    [Title("Current Values")] [SerializeField]
    private BaseGun currentGun;


    [Title("Abilities")]
    private List<IEffect<IDamageable>> _currentEffects;

    [Title("Debug Values")] public BaseGun testGun;
    [SerializeField,SerializeReference] private List<Ability> debugAbilities;
    [SerializeField] private FishingRod fishingRod;

    private event Action leftClickInteraction;
    private event Action upLeftClickInteraction;
    private event Action rightClickInteraction;
    private event Action upRightClickInteraction;
    private Action interactAction;


    //The current tool that is being used
    //This can be either the fishing pole or a gun or something else
    private Tool _currentTool;
    
    private Transform _currentlyHitting;
    // Start is called once before the first execution of Update after the MonoBehaviour is create


    private void Start()
    {
        //Test Ability
        if (debugAbilities.Count > 0)
        {
            foreach (var a in debugAbilities)
            {
                //FeedAbility(a.Create());
            }
        }

        //Setting the current hitting transform to the player's transform for the first time 
        _currentlyHitting = transform;
        
        _currentEffects = new List<IEffect<IDamageable>>();
        
        ServiceLocator.Instance.AddService(this);
        //FeedGun(testGun);
    }


    public void FeedUseableItem(IPlayerUseable useableItem)
    {
        if (useableItem is Tool tool)
        {
            
            //Unsubscribe the current tool from the events
            _currentTool.OnDeselect();
            
            
            if (_currentTool != null)
            {
                Destroy(_currentTool.gameObject);
            }

            
            Tool go = Instantiate(tool, itemHolder);

            go.transform.SetParent(itemHolder);
            
            go.transform.localPosition = tool.transform.localPosition;
            go.transform.localRotation = tool.transform.localRotation;
            
            
            _currentTool = go;
            
            
            
            //Set the 
            _currentTool.OnSelect();
            
            //Subscribe to the events 
            if (tool is BaseGun gun)
            {
                currentGun = gun;
            }
        }
    }


    public void FeedAbility(IEffect<IDamageable> effect)
    {
        _currentEffects.Add(effect);
    }


    void Update()
    {
        foreach (var currentEffect in _currentEffects)
        {
            currentEffect.OnUpdateTick(Time.deltaTime);
        }
     
        
        
        //Interaction ---------
        CheckForInteraction();
        
        if (InputWrapper.Instance.performInteract.triggered)
            interactAction?.Invoke();
    }

    
    //TODO:: QUick and dirty, refactor
    private void CheckForInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5f, interactLayerMask))
        {
            if (_currentlyHitting.transform == hit.transform) return;

            Debug.Log($"Hit {hit.transform.name}");
            _currentlyHitting = hit.transform;

            _currentlyHitting.transform.TryGetComponent<IOnInteract>(out var interactable);


            Debug.Log(interactable);
            if (interactable == null) return;


            interactAction += interactable.OnInteract;


            if (ServiceLocator.Instance.Locate(out UIManager uiManager))
            {
                uiManager.InteractStatus(true);
            }

            return;
        }


        if (ServiceLocator.Instance.Locate(out UIManager ui))
            ui.InteractStatus(false);

        //Reset the current hitting transform
        _currentlyHitting = transform;

        interactAction = null;
    }


    public void GetAbility(Ability ability)
    {
        switch (ability.effectType)
        {
            case Ability.EffectType.Self:

                break;
            case Ability.EffectType.Weapon:
                //Feed To Gun instead
                currentGun.FeedAbility(ability);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }



    [ContextMenu("Add Fishing Pole")]
    public void AddFishingPole()
    {
        FeedUseableItem(fishingRod);
    }

    public void ServiceAdded()
    {
    }
    public void RemoveService()
    {
    }
    public void OnLocate()
    {
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);
    }
}