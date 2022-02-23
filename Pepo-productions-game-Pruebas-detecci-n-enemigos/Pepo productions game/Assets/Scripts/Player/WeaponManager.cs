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
        public int bulletSpeed;
    };


    [HideInInspector]
    public Vector2 mouseScroll;

    public int currentWeapon = 0;
    public const int totalWeapons = 1;
    public Weapon[] weapons = new Weapon[totalWeapons];

    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
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
