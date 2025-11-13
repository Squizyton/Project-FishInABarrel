using System;
using System.Collections;
using System.Collections.Generic;
using Alchemy.Inspector;
using Damage;
using Damage.Factories;
using Interfaces;
using Structs;
using UnityEngine;

public class Fish : MonoBehaviour, IDamageable, IHittable
{
    [Title("Stats")] [SerializeField] private float health;
    [SerializeField] private float baseCashValue;

    [Title("Damage Handlers")] [SerializeReference]
    public List<IProcessorFactory<IDamageable>> damageProcessors = new List<IProcessorFactory<IDamageable>>();

    [Title("RB Stats")] [SerializeField] private float maxMagnitude;
    [SerializeField] private float baseLaunchValue;

    [Title("RB Stats/Max Values")] [SerializeField]
    private float maxHeight;


    [Title("Multiplyer")]
    [ReadOnly] protected float multiplier;

    [ReadOnly] protected int juggleComboCount;

    [Title("References")] private Rigidbody rb;

    [Title("Particles")] [SerializeField] private GameObject bloodParticles;


    private bool _isPrimed;

    private IDamageProcessor<IDamageable> _processorBaseOfChain;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        
        //Debug
        damageProcessors.Add(new BaseDamageFactory());
        
        
        
        //Set up the Damage Processor chain

        IDamageProcessor<IDamageable> currentProcessor = null;

        foreach (var processorFactory in damageProcessors)
        {
            var damageProcessor = processorFactory.Create();

            if (currentProcessor != null)
            {
                currentProcessor.SetNext(damageProcessor);
                Debug.Log("Creating:" + currentProcessor.GetType().Name);
            }
            else
            {
                _processorBaseOfChain = damageProcessor;
                currentProcessor = damageProcessor;
            }
        }
    }


    protected virtual void Update()
    {
        if (_isPrimed && Physics.SphereCast(transform.position, 0.1f, Vector3.down, out RaycastHit hit, 0.5f))
        {
            OnDeath();
        }


        ClampVelocity();
        ClampHeight();
    }

    private void ClampVelocity()
    {
        if (rb.linearVelocity.magnitude > maxMagnitude)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxMagnitude;
        }
    }

    private void ClampHeight()
    {
        if (transform.position.y > maxHeight)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, maxHeight, transform.position.z),
                5 * Time.deltaTime);
        }
    }

    public virtual void OnHit(HitInfo damageInfo)
    {


        rb.AddForce(Vector3.up * (baseLaunchValue * (damageInfo.Damage * 10)), ForceMode.Impulse);

        rb.AddForce(damageInfo.Hit.normal * 3, ForceMode.Impulse);
        

        //Add torqie base on the direction of the hit
        rb.AddTorque(Vector3.Cross(transform.position, damageInfo.Hit.normal) * 10, ForceMode.Impulse);
        
        
        //If hit once after launch, prime it so if it hits the ground again, call destroy
        if (!_isPrimed)
            StartCoroutine(PrimeCountDown());

        juggleComboCount++;
        
        //Spawn blood particles
        var blood = Instantiate(bloodParticles, transform.position, Quaternion.identity);
        //Set the rotation to the normal
        blood.transform.rotation = Quaternion.LookRotation(damageInfo.Hit.normal);

        //Start the damage processor chain
        _processorBaseOfChain?.ProcessDamage(this, new DamageData
        {
            Damage = damageInfo.Damage
        });
    }


    private readonly WaitForSeconds _primeCacheWaitFor = new WaitForSeconds(0.4f);

    public IEnumerator PrimeCountDown()
    {
        yield return _primeCacheWaitFor;
        _isPrimed = true;
    }

    protected virtual void OnDeath()
    {

        //If Dies it should just add 2 to the multiplier
        //THIS COULD BE A DAMAGE PROCESSOR
        multiplier *= 2 * juggleComboCount;
        
        GameManager.Instance.inventory.AddCash(baseCashValue, multiplier);

        
        //Explosion particles
        Destroy(gameObject);
    }

    public void OnDamage(float damageData)
    {
        health -= damageData;
        
        
        multiplier += damageData / 10;
        Debug.Log($"Damage Data {damageData} : " + multiplier);
        if (health <= 0)
        {
            OnDeath();
        }
    }
}