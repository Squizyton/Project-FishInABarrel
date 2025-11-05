using Interfaces;
using Structs;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Utilities;

namespace Guns.Guns
{
    public class Glock : BaseGun
    {
        [SerializeField] private Animator anim;
        [SerializeField] private GameObject muzzleFlash;

        public override void OnFire()
        {
            if (ammoCount > 0)
            {
                Debug.Log("Fired Glock");
                
                var mainCamera = StaticUtilities.GetMainCamera();
                //CameraShake.Shake(0.5f, 0.5f, 0.15f);
                //muzzleFlash.SetActive(true);

                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out var hit,
                        Mathf.Infinity, enemyLayerMask))
                {
                    if (hit.transform.gameObject.layer == 3)
                    {
                        Debug.Log("Spawning Decal");
                        var decal = Instantiate(bulletHoleDecal, hit.point, Quaternion.identity);
                        
                        
                        decal.transform.forward = -hit.normal;


                        var offsetPosition = decal.transform.position;
                        offsetPosition.x += .3f;

                        decal.transform.position = offsetPosition;
                    }


                    TrailRenderer trail = Instantiate(trailRenderer, bulletSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));
                    
                    hit.transform.TryGetComponent(out IHittable damageableObject);

                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        OnHitWithAbility(damageable);
                    }

                    
                    HitInfo newInfo = new HitInfo
                    { 
                        Damage = damage,
                        Hit = hit
                    };
                    
                    
                    
                    damageableObject?.OnHit(newInfo);
                }
                else
                {
                    Debug.Log("No Hit");
                    TrailRenderer trail = Instantiate(trailRenderer, bulletSpawnPoint.position, Quaternion.identity);
                    
                    //Make the target point 100 units away from the camera
                    StartCoroutine(SpawnTrail(trail, StaticUtilities.GetMainCamera().transform.position + StaticUtilities.GetMainCamera().transform.forward * 100,3f));
                }

        
            }

            base.OnFire();
        }
    }
}
