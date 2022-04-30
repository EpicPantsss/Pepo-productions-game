using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponInfo : MonoBehaviour
{
    public int weaponID = 0;
    public string weaponName = "";
    public int weaponDamage = 1;
    public float attackRecoil = 0.1f;
    public float attackSpeed;
    public AudioClip attackSound;
    public AudioClip recoverSound;
    public string[] leftAnimations = new string[2];
    public string[] rightAnimations = new string[2];
    public string[] downAnimations = new string[2];
}
