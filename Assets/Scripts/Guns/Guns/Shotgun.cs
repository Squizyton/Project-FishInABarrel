using Alchemy.Inspector;
using Interfaces;

using Structs;
using Unity.VisualScripting;
using UnityEngine;
using Utilities.Utilities;

namespace Guns.Guns
{
    public class Shotgun : BaseGun
    {
        [Title("Shotgun Specific Values")] [SerializeField]
        private int bulletsPerShell;

        
        [SerializeField, Range(0,1)] private float spreadAngle;
        public override void OnFire()
        {

            if (ammoCount > 0)
            {
                Debug.Log("Fired Shotgun");
                for (int i = 0; i < bulletsPerShell; i++)
                {
                    Vector3 randomOffset = Random.insideUnitSphere * (spreadAngle + (bulletsPerShell / 100f)) ;
                    Vector3 spreadDirection = StaticUtilities.GetMainCamera().transform.forward + randomOffset;
                    
                    float rayLength = 10.0f; // Adjust length as needed
                    Color color = Color.red; // Choose your debug color
                    
                    Debug.DrawRay(bulletSpawnPoint.position, spreadDirection * rayLength, color);
                    
                    // Fire the raycast
                    if (Physics.Raycast(transform.position, spreadDirection, out RaycastHit hit,20f))
                    {
                        hit.transform.TryGetComponent(out IHittable damageableObject);

                        if (hit.transform.TryGetComponent(out IDamageable damageable))
                        {
                            OnHitWithAbility(damageable);
                        }

                        damageableObject?.OnHit(new HitInfo
                        {
                            Damage = damage,
                            Hit = hit
                        });
                        
                        
                        
                        
                        TrailRenderer trail = Instantiate(trailRenderer, bulletSpawnPoint.position, Quaternion.identity);
                        StartCoroutine(SpawnTrail(trail, hit));
                        
                    }
                    else
                    {
                        TrailRenderer trail = Instantiate(trailRenderer, bulletSpawnPoint.position, Quaternion.identity);
                        StartCoroutine(SpawnTrail(trail, transform.position + spreadDirection * 20));
                    }
                }
     
            }

            base.OnFire();
        }
    }
}