using Alchemy.Inspector;
using Interfaces;
using Structs;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.Utilities;

namespace Guns.Guns
{
    public class Mac10 : BaseGun
    {
        [Title("Mac 10 Specific Values")]
        [SerializeField]private float spreadAngle;
        
        
        private float _timer;
        private bool _canFire;
        [SerializeField] private GameObject muzzleFlash;
        public override void OnFire()
        {
            if (_canFire && ammoCount > 0)
            {
                
                    
                Vector3 randomOffset = Random.insideUnitSphere * spreadAngle;
                Vector3 spreadDirection = StaticUtilities.GetMainCamera().transform.forward + randomOffset;

                float rayLength = 10.0f; // Adjust length as needed
                Color color = Color.red; // Choose your debug color

                Debug.DrawRay(transform.position, spreadDirection * rayLength, color);
                muzzleFlash.SetActive(true);
                
                // Fire the raycast
                if (Physics.Raycast(transform.position, spreadDirection, out RaycastHit hit))
                {
                    hit.transform.TryGetComponent(out IHittable damageableObject);
                    damageableObject?.OnHit(new HitInfo
                    {
                        Damage = damage,
                        Hit = hit
                    });
                    
                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        OnHitWithAbility(damageable);
                    }

                    
                    
                        
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
                }
                _timer = fireRate;
                _canFire = false;
                
                base.OnFire();
            }
            else if (!_canFire && ammoCount > 0)
            {
                if (_timer > 0f)
                {
                    _timer -= Time.deltaTime;
                }
                else
                {
                    _canFire = true;
                }
            }
            
            if(ammoCount <=0)
                base.OnFire();
                

        }
        
        public override void OnLeftClick(InputAction.CallbackContext ctx)
        {
            OnFire();
        }
    }
}
