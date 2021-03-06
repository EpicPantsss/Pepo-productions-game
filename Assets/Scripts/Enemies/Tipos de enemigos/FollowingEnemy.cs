using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDamage))]
[RequireComponent(typeof(EnemyDetection))]
[RequireComponent(typeof(Rigidbody2D))]
public class FollowingEnemy : MonoBehaviour
{
    public float enemySpeed;

    private GameObject player;

    private float distanceToRotate;

    private EnemyDetection enemyDetection;
    private EnemyDamage enemyDamage;

    //Animaci?n
    private Animator anim;

    private Rigidbody2D rb;


    void Start()
    {
        player = GameObject.Find("Player");
        enemyDetection = GetComponent<EnemyDetection>();
        enemyDamage = GetComponent<EnemyDamage>();

        anim = GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (enemyDetection.playerDetected && !enemyDamage.death)
        {
            anim.SetBool("Walking", true);
            distanceToRotate = getAngle(transform.position, player.transform.position);
            // =================
            // Rotaci?n del enemigo
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);

            float getAngle(Vector2 position, Vector2 mousePosition)
            {
                float x = mousePosition.x - position.x;
                float y = mousePosition.y - position.y;

                return Mathf.Rad2Deg * Mathf.Atan2(y, x);
            }
            // =================

            // Con esto el enemigo se mover? hacia adelante
            rb.velocity = Vector2.right * enemySpeed * Time.deltaTime;
        }

        if (enemyDetection.playerJustUndetected)
        {
            rb.velocity = Vector2.right * enemySpeed * Time.deltaTime;
        }

        if (enemyDetection.playerDetected && enemyDetection.playerJustUndetected)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
}