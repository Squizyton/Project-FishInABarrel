using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Stats", menuName = "New Gun Stats")]
public class GunStats : ScriptableObject
{
    public int ammo;
    public float damage;
    public bool AutoFire;
    public float recoil;
    public float fireRate;
}
