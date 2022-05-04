using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanguinario : MonoBehaviour
{
    public int healAmount;
    private PlayerAttack playerAttack;

    private int bulletNumber;
    // Start is called before the first frame update
    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();

        bulletNumber = playerAttack.bulletsToInit;
        for (int i = 0; i < bulletNumber; i++)
        {
            playerAttack.bulletRepositoryScripts[i].sanguinary = true;
        }
    }
}
