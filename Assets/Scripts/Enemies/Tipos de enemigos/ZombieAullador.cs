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

    private float distanceToRotate;

    private GameObject player;

    private CircleCollider2D triggerZone;
    void Start()
    {
        player = GameObject.Find("Player");

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

            // =================
            // Rotación del enemigo
            distanceToRotate = getAngle(transform.position, player.transform.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);

            float getAngle(Vector2 position, Vector2 mousePosition)
            {
                float x = mousePosition.x - position.x;
                float y = mousePosition.y - position.y;

                return Mathf.Rad2Deg * Mathf.Atan2(y, x);
            }
            // =================

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
