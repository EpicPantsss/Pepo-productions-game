using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public uint weaponID = 0;
    public string weaponName = "";
    public int weaponDamage = 1;
    public float fireRecoil = 0.1f;
    public int weaponAmmo = 6;
    public float bulletSpeed;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public List<string> animationNames;
    public WeaponManager.AmmoTypes ammoType;
}
