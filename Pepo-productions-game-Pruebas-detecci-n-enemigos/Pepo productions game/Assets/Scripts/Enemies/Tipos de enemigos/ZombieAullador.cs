using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ZombieAullador : MonoBehaviour
{
    private EnemyDetection enemyDetection;

    public bool scream;
    private float detectionRange;
    private float aux;

    private CircleCollider2D triggerZone;
    void Start()
    {
        enemyDetection = GetComponent<EnemyDetection>();

        detectionRange = enemyDetection.detectionRadius;

        triggerZone = GetComponent<CircleCollider2D>();

        triggerZone.radius = 0;
        triggerZone.isTrigger = true;
    }

    void Update()
    {
        if (enemyDetection.playerDetected)
        {
            scream = true;

            if (aux < detectionRange)
            {
                aux++;
                triggerZone.radius = aux;
            }
        }
        else
        {
            scream = false;

            triggerZone.radius = 0;

            if (aux > 0)
            {
                aux--;
                triggerZone.radius = aux;
            }
        }
    }
}
