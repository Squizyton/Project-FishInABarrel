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


    private Action leftClickInteraction;
    private Action interactAction;


    private IPlayerUseable _useableItem;
    
    //Easy caching
    private BaseGun _currentSpawnedGun;
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
        ServiceLocator.Instance.AddService(this);
        //FeedGun(testGun);
    }


    public void FeedUseableItem(IPlayerUseable useableItem)
    {
   
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
        
        if (InputWrapper.Instance.performLeftClick.triggered)
        {
            Debug.Log("Left Click");
            leftClickInteraction?.Invoke();
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