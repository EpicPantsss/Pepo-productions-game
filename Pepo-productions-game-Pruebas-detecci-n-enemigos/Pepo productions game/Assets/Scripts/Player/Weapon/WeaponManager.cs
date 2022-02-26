using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [HideInInspector]
    public Vector2 mouseScroll;
    public int weaponsOnInventory = 1;

    public int currentWeapon = 0;


    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        // Detecta si el jugador est� moviendo la rueda del rat�n, y en que direcci�n
        mouseScroll = Input.mouseScrollDelta;

        if (mouseScroll.y == 1 && currentWeapon < 3)
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
