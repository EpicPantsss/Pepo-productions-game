using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float detectionRadius = 10.0f;
    public float detectionAngle = 90.0f;

    public Transform player;

    public Vector2 toPlayer;

    public bool playerDetected;

    private void Update()
    {
        toPlayer = player.localPosition - transform.localPosition;


        if (toPlayer.magnitude <= detectionRadius)
        {
            if (Vector2.Angle(transform.right, player.position - transform.position) < detectionAngle)
            {
                playerDetected = true;
            }
            else
            {
                playerDetected = false;
            }
        }
        else
        {
            playerDetected = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
