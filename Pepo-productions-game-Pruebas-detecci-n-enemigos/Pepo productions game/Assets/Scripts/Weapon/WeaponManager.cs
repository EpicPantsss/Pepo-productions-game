using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum AmmoTypes { PISTOL, DEAGLE, AK, LAST_NO_USE };

    [HideInInspector]
    public Vector2 mouseScroll;
    public int weaponsOnInventory = 1;

    public int currentWeapon = 0;
    [Header("IMPORTANTE: Orden de las armas igual que el enum")]
    [Header("Lista con todas las armas")]
    public List<WeaponInfo> allWeapons;

    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        // Detecta si el jugador está moviendo la rueda del ratón, y en que dirección
        mouseScroll = Input.mouseScrollDelta;

        if (mouseScroll.y == 1 && currentWeapon < weaponsOnInventory)
        {
            currentWeapon++;
            playerAttack.ChangeWeapon(currentWeapon);
            
        }
        if (mouseScroll.y == -1 && currentWeapon > 0)
        {
            currentWeapon--;
            playerAttack.ChangeWeapon(currentWeapon);
        }
    }

}
