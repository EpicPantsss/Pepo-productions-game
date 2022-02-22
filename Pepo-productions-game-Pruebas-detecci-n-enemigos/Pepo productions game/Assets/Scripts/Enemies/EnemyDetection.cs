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

    public float timer;

    [HideInInspector]
    public bool playerJustUndetected;


    private void Update()
    {
        toPlayer = player.localPosition - transform.localPosition;

        if (toPlayer.magnitude <= detectionRadius)
        {
            if (Vector2.Angle(transform.right, player.position - transform.position) < detectionAngle)
            {
                playerDetected = true;
                playerJustUndetected = false;
                timer = 0;
            }
            else
            {
                // Si el enemigo estaba viendo al jugador y justo deja de verlo, pondr� en true a playerJustUndetected
                if (playerDetected)
                {
                    playerJustUndetected = true;
                }

                playerDetected = false;
            }
        }
        else
        {
            // Si el enemigo estaba viendo al jugador y justo deja de verlo, pondr� en true a playerJustUndetected
            if (playerDetected)
            {
                playerJustUndetected = true;
            }

            playerDetected = false;
        }

        if (playerJustUndetected)
        {
            timer += Time.deltaTime;

            if (timer >= 3)
            {
                playerJustUndetected = false;
                timer = 0;
            }
        }
    }


    private void OnDrawGizmos()
    {
        // Para solo dibujar el rango de detecci�n si se selecciona el objeto
        if (UnityEditor.Selection.activeGameObject != this.gameObject 
            && UnityEditor.Selection.activeGameObject != this.gameObject.transform.GetChild(0).gameObject) { 
            return; 
        }
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
