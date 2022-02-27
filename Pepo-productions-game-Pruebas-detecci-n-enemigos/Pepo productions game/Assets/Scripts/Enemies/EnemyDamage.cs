using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int enemyHP;

    private bool damaged;

    private float timer;

    private EnemyDetection enemyDetection;

    public bool called;
    bool hasPatrolScript;

    private void Start()
    {
        enemyDetection = GetComponent<EnemyDetection>();

        hasPatrolScript = enemyDetection.hasPatrolScript;
    }

    private void Update()
    {
        if (damaged)
        {
            timer += Time.deltaTime;
            if (timer > 0.025f)
            {
                damaged = false;
                timer = 0;
            }
        }

        if (called)
        {
            GetComponent<EnemyPatrol>().enabled = false;
            enemyDetection.playerDetected = true;
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

        if (other.CompareTag("Aullador"))
        {
            called = true;
            if (hasPatrolScript)
            {
                GetComponent<EnemyPatrol>().enabled = false;
                GetComponent<EnemyPatrol>().playerSaw = false;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Aullador"))
        {
            called = false;
            GetComponent<EnemyPatrol>().enabled = true;
            GetComponent<EnemyPatrol>().playerSaw = false;
        }
    }
}
