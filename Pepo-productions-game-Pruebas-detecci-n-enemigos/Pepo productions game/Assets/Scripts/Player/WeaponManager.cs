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

    public Weapon[] weapons;

    [HideInInspector]
    public Vector2 mouseScroll;

    public int currentWeapon = 0;

    private void Start()
    {
        Debug.Log(weapons);
    }

    private void Update()
    {
        // Detecta si el jugador está moviendo la rueda del ratón, y en que dirección
        mouseScroll = Input.mouseScrollDelta;

        if (mouseScroll.y == 1)
        {
            currentWeapon++;
        }
        if (mouseScroll.y == -1)
        {
            currentWeapon--;
        }
    }

}
