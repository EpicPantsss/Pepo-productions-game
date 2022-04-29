using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyDamage))]
[RequireComponent(typeof(EnemyDetection))]
[RequireComponent(typeof(Rigidbody2D))]
public class DogZombie : MonoBehaviour
{
    public float enemySpeed;

    private GameObject player;

    private float distanceToRotate;

    private EnemyDetection enemyDetection;
    private EnemyDamage enemyDamage;

    //Animación
    private Animator anim;

    private Rigidbody2D rb;

    private float attackRange;
    // Ataque en salto
    [Header("Fuerza del salto")]
    [SerializeField] float jumpForce;
    private bool onAttack;

    //Timer
    private float timer;


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
        if (enemyDetection.playerDetected && !enemyDamage.death && !onAttack)
        {
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

            /// Ataque en salto
            if (enemyDetection.toPlayer <= attackRange)
            {
                rb.AddForce(Vector2.right * jumpForce, ForceMode2D.Impulse);
                onAttack = true;
            }
            else /// Movimiento normal
            {
                anim.SetBool("Walking", true);

                // Con esto el enemigo se moverá hacia adelante
                rb.velocity = Vector2.right * enemySpeed;
            }
        }

        if (enemyDetection.playerJustUndetected)
        {
            rb.velocity = Vector2.right * enemySpeed;
        }

        if (enemyDetection.playerJustUndetected)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        // Timer
        if (onAttack)
        {
            timer += Time.deltaTime;
            if (timer > 1.5f)
            {
                onAttack = false;
                timer = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject != this.gameObject)
            return;
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (onAttack)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
}