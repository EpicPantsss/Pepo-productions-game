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

    [Header("N�mero de armas")]
    public int weaponNumber;

    public List<Weapon> weaponsEquipped;
    private Weapon[] totalWeapons;

    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

    void InitWeapons()
    {
        totalWeapons = new Weapon[weaponNumber];
    }

    private void Update()
    {
        // Detecta si el jugador est� moviendo la rueda del rat�n, y en que direcci�n
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
