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
            Debug.Log(Vector3.Dot(toPlayer, Vector2.right));
            if (Vector3.Dot(toPlayer, Vector2.right) >
                Mathf.Cos(detectionAngle * player.position.x * player.position.y * 0.5f * Mathf.Deg2Rad))
            {
                playerDetected = true;
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
