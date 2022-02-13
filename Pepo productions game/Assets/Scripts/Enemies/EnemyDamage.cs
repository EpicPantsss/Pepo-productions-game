using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int enemyHP;

    private bool damaged;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !damaged)
        {
            enemyHP--;
            if (enemyHP <= 0)
            {
                Destroy(gameObject);
            }
            StartCoroutine(ImmortalTime());
        }
    }

    IEnumerator ImmortalTime()
    {
        damaged = true;
        yield return new WaitForSeconds(0.01f);
        damaged = false;
    }
}
