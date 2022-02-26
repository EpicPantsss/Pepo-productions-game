using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int enemyHP;

    private bool damaged;

    private float timer;


    private void Update()
    {
        if (damaged)
        {
            timer += Time.deltaTime;
            if (timer > 0.075f)
            {
                damaged = false;
                timer = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !damaged)
        {
            enemyHP -= other.gameObject.GetComponent<Bullet>().bulletDamage;
            if (enemyHP <= 0)
            {
                Destroy(gameObject);
            }
            damaged = true;
        }
    }
}
