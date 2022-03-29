using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocista : MonoBehaviour
{
    public float speedIncreaseAmount;

    private GameObject player;

    private PlayerMovement playerMovement;
    private PlayerDamage playerDamage;

    private int initialHP;
    void Start()
    {
        player = GameObject.Find("Player");

        playerMovement = player.GetComponent<PlayerMovement>();
        playerDamage = player.GetComponent<PlayerDamage>();

        initialHP = playerDamage.playerHP;
    }

    void Update()
    {
        playerMovement.speed = playerMovement.speed * (initialHP / playerDamage.playerHP);
    }
}
