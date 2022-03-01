using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDamage : MonoBehaviour
{
    public int enemyHP;

    private bool damaged;

    private float timer;

    private EnemyDetection enemyDetection;

    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;
    public AudioClip deathSound;

    public bool called;
    bool hasPatrolScript;
    [HideInInspector]
    public bool death;

    private Rigidbody2D rb;

    private void Start()
    {
        enemyDetection = GetComponent<EnemyDetection>();

        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

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
            timer += Time.deltaTime;
            if (timer > 5)
            {
                called = false;
                timer = 0;
            }
        }
        if (death || enemyHP <= 0)
        {
            spriteRenderer.enabled = false;
            
            timer += Time.deltaTime;
            if (timer > 1.5f)
            {
                Destroy(gameObject);
            }
        }

        if (called)
        {
            if (hasPatrolScript)
                GetComponent<EnemyPatrol>().enabled = false;
            enemyDetection.playerDetected = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !damaged)
        {
            if (other.transform.parent != null)
            {
                enemyHP -= other.gameObject.GetComponent<Bullet>().bulletDamage;
            }
            if (enemyHP <= 0)
            {
                audioSource.clip = deathSound;
                audioSource.Play();
                spriteRenderer.enabled = false;
                death = true;
            }
            called = true;
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

    public void DecreaseSpeed()
    {
        rb.velocity = new Vector2(rb.velocity.x - 5 * Time.deltaTime, rb.velocity.y - 5 * Time.deltaTime);
        if (rb.velocity.x > 0 || rb.velocity.y > 0)
        {
            DecreaseSpeed();
        }
    }
}
