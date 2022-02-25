using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public struct Weapon
    {
        public int weaponID;
        public string weaponName;
        public int weaponDamage;
        public int weaponAmmo;
        public float weaponRecoil;
        public int bulletSpeed;
    };


    [HideInInspector]
    public Vector2 mouseScroll;
    public int weaponsOnInventory = 1;

    public int currentWeapon = 0;

    [Header("Número de armas")]
    public int weaponNumber;

    public List<Weapon> weaponsEquipped;
    private Weapon[] totalWeapons;

    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();

        InitWeapons();
    }

    void InitWeapons()
    {
        totalWeapons = new Weapon[weaponNumber];
        totalWeapons[0].weaponID = 0;
        totalWeapons[0].weaponName = "Pistol";
        totalWeapons[0].weaponDamage = 1;
        totalWeapons[0].weaponRecoil = 0.2f;
        totalWeapons[0].bulletSpeed = 30;
        totalWeapons[0].weaponID = 1;
        totalWeapons[0].weaponAmmo = 10;

        totalWeapons[1].weaponName = "Eagle";
        totalWeapons[1].weaponDamage = 2;
        totalWeapons[1].weaponRecoil = 1f;
        totalWeapons[1].bulletSpeed = 30;
        totalWeapons[1].weaponAmmo = 3;

        weaponsEquipped.Capacity = weaponsOnInventory;
        for (int i = 0; i < weaponsOnInventory; i++)
        {
            weaponsEquipped.Add(totalWeapons[i]);
        }
    }

    private void Update()
    {
        // Detecta si el jugador está moviendo la rueda del ratón, y en que dirección
        mouseScroll = Input.mouseScrollDelta;

        if (mouseScroll.y == 1)
        {
            currentWeapon++;
            playerAttack.ChangeWeapon(currentWeapon);
            
        }
        if (mouseScroll.y == -1)
        {
            currentWeapon--;
            playerAttack.ChangeWeapon(currentWeapon);
        }
    }

}
