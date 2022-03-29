using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetection))]
public class FollowingEnemy : MonoBehaviour
{
    public float enemySpeed;

    private GameObject player;

    private float distanceToRotate;

    private EnemyDetection enemyDetection;
    private EnemyDamage enemyDamage;

    //Animación
    private Animator anim;


    void Start()
    {
        player = GameObject.Find("Player");
        enemyDetection = GetComponent<EnemyDetection>();
        enemyDamage = GetComponent<EnemyDamage>();

        anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if (enemyDetection.playerDetected && !enemyDamage.death)
        {
            anim.SetBool("Walking", true);
            distanceToRotate = getAngle(transform.position, player.transform.position);
            // =================
            // Rotación del enemigo
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);

            float getAngle(Vector2 position, Vector2 mousePosition)
            {
                float x = mousePosition.x - position.x;
                float y = mousePosition.y - position.y;

                return Mathf.Rad2Deg * Mathf.Atan2(y, x);
            }
            // =================

            // Con esto el enemigo se moverá hacia adelante
            transform.Translate(Vector2.right * enemySpeed * Time.deltaTime);
        }

        if (enemyDetection.playerJustUndetected)
        {
            transform.Translate(Vector2.right * enemySpeed * Time.deltaTime);
        }

        if (enemyDetection.playerDetected || enemyDetection.playerJustUndetected)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }
}