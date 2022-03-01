using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float detectionRadius = 10.0f;
    public float detectionAngle = 90.0f;

    private float aux;

    public Transform player;

    public Vector2 toPlayer;

    public bool playerDetected;

    public float timer;

    [HideInInspector]
    public bool playerJustUndetected;

    public float distanceToWalk;
    public Vector2 playerLastPositionSeen;


    private EnemyPatrol enemyPatrol;
    private EnemyDamage enemyDamage;
    [HideInInspector]
    public bool hasPatrolScript;
    [HideInInspector]
    public bool knocked;

    private float originalDetectionAngle;
    private float playerDetectedAngle;

    private void Awake()
    {
        enemyDamage = GetComponent<EnemyDamage>();
        // Intentas obtener el script de patrulla, y si el enemigo lo tiene, lo guarda en la variable y se marca hasPatrolScript como true
        enemyPatrol = GetComponent<EnemyPatrol>();
        if (enemyPatrol != null)
        {
            hasPatrolScript = true;
        }

        originalDetectionAngle = detectionAngle;
        playerDetectedAngle = detectionAngle + 20;
    }

    private void Update()
    {
        toPlayer = player.localPosition - transform.localPosition;

        if (!enemyDamage.called && !knocked)
        {
            if (toPlayer.magnitude <= detectionRadius)
            {
                // Raycast para detectar si hay un objeto delante del jugador
                RaycastHit2D rayToPlayer = Physics2D.Raycast(transform.position, player.position - transform.position);

                if (rayToPlayer.collider.tag == "Player")
                {
                    Debug.DrawRay(transform.position, player.position - transform.position, Color.green);

                    if (Vector2.Angle(transform.right, player.position - transform.position) < detectionAngle)
                    {
                        playerDetected = true;
                        playerJustUndetected = false;
                        timer = 0;
                        playerLastPositionSeen = player.position;
                        // Aumentar el rango de visión del player
                        detectionAngle = playerDetectedAngle;

                        // Si el enemigo está patrullando, dejará de hacerlo
                        if (hasPatrolScript)
                            enemyPatrol.playerSaw = true;
                    }
                    else
                    {
                        // Si el enemigo estaba viendo al jugador y justo deja de verlo, pondrá en true a playerJustUndetected
                        if (playerDetected)
                        {
                            playerJustUndetected = true;
                        }

                        playerDetected = false;

                        // Si el enemigo estaba patrullando y deja de ver al jugador, volverá a patrullar
                        if (hasPatrolScript)
                            enemyPatrol.playerSaw = false;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, player.position - transform.position, Color.red);
                    if (playerDetected)
                    {
                        playerJustUndetected = true;
                    }

                    playerDetected = false;

                    // Si el enemigo estaba patrullando y deja de ver al jugador, volverá a patrullar
                    if (hasPatrolScript)
                        enemyPatrol.playerSaw = false;
                }

            }
            else
            {
                // Si el enemigo estaba viendo al jugador y justo deja de verlo, pondrá en true a playerJustUndetected
                if (playerDetected)
                {
                    playerJustUndetected = true;
                }

                playerDetected = false;
            }
        }
        else
        {
            playerDetected = true;
        }

        if (playerJustUndetected)
        {
            timer += Time.deltaTime;
            distanceToWalk = Vector2.Distance(transform.position, playerLastPositionSeen);

            if (timer >= 3 || distanceToWalk <= 0)
            {
                playerJustUndetected = false;
                timer = 0;
                distanceToWalk = 0;

                detectionAngle = originalDetectionAngle;

                if (hasPatrolScript)
                {
                    enemyPatrol.playerSaw = false;
                    enemyPatrol.GetDirection();
                }
            }
        }
    }


    private void OnDrawGizmos()
    {       
        /*
        // Para solo dibujar el rango de detección si se selecciona el objeto
        if (UnityEditor.Selection.activeGameObject != this.gameObject 
            && UnityEditor.Selection.activeGameObject != this.gameObject.transform.GetChild(0).gameObject) { 
            return; 
        }
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.white;
        float halfFOV = detectionAngle;
        float coneDirection = 0;

        Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
        Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

        Vector3 upRayDirection = upRayRotation * transform.right * detectionRadius;
        Vector3 downRayDirection = downRayRotation * transform.right * detectionRadius;

        Gizmos.DrawRay(transform.position, upRayDirection);
        Gizmos.DrawRay(transform.position, downRayDirection);
        */
    }
}
