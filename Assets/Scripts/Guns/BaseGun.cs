using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using Alchemy.Inspector;
using Interfaces;

using UnityEngine;


namespace Guns
{
    public abstract class BaseGun : MonoBehaviour
    {
        [Title("Gun Data")] [SerializeField] private GunStats stats;

        [Title("Gun Values")] [SerializeField, ReadOnly]
        protected int ammoCount;

        [SerializeField, ReadOnly] protected float damage;
        [SerializeField, ReadOnly] protected bool automatic;
        [SerializeField, ReadOnly] protected float recoil;
        [SerializeField, ReadOnly] protected float fireRate;
        [SerializeField] protected float tossDamage;


        [Title("Sound")] [SerializeField] private AudioClip sound;
        [SerializeField] private AudioConfiguration audioConfig;

        [Title("Details")] [SerializeField] protected GameObject bulletHoleDecal;
        [SerializeField] protected LayerMask enviroLayerMask;
        [SerializeField] protected TrailRenderer trailRenderer;
        [SerializeField] protected Transform bulletSpawnPoint;
        [SerializeField] protected GameObject sparksParticle;

        
        [Title("Abilities")]

        [SerializeField] private List<Ability> abilities;
        
        private bool _tossed;

        [Title("Layer Mask")] [SerializeField] protected LayerMask enemyLayerMask;


        public void OnEnable()
        {
            ammoCount = stats.ammo;
            damage = stats.damage;
            automatic = stats.AutoFire;
            recoil = stats.recoil;
            fireRate = stats.fireRate;
        }

        public void FeedAbility(Ability ability)
        {
            abilities.Add(ability);
        }


        public virtual void OnFire()
        {
            if (ammoCount <= 0) return;
            
            Debug.Log("Fired");
            ammoCount--;
            FireEffect();
        }

        public virtual void OnHitWithAbility(IDamageable target)
        {
        }

        public virtual void FireEffect()
        {
        }

        public int ReturnAmmo()
        {
            return ammoCount;
        }


        protected IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        {
            float time = 0;
            Vector3 startPosition = trail.transform.position;

            while (time < 1)
            {
                trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }

            trail.transform.position = hit.point;
            //var particle = Instantiate(sparksParticle, hit.point, Quaternion.identity);
            //particle.transform.forward = -hit.normal;


            //instantiate bullet holes here


            Destroy(trail.gameObject, trail.time);
        }

        protected IEnumerator SpawnTrail(TrailRenderer trail, Vector3 position, float timeToLive = 1)
        {
            float time = 0;
            Vector3 startPosition = trail.transform.position;

            while (time < timeToLive)
            {
                trail.transform.position = Vector3.Lerp(startPosition,position, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }

            trail.transform.position = position;


            //instantiate bullet holes here


            Destroy(trail.gameObject, trail.time);
        }

        public bool IsAuto()
        {
            return automatic;
        }
    }
}