using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    public int playerHP;

    public Image hpImage;

    private bool damaged;
    private float timer;

    void Update()
    {
        if (damaged)
        {
            // Tiempo de invulnerabilidad del jugador
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                damaged = false;
                timer = 0;
            }
        }
    }

    public void IncreaseHP(int amount)
    {
        if (!damaged)
        {
            playerHP += amount * 2;
            hpImage.fillAmount = playerHP * 0.1f;

            damaged = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !damaged)
        {
            playerHP--;
            hpImage.fillAmount = playerHP * 0.1f;
            damaged = true;
        }
    }
}
