using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool death;


    private void Start()
    {
        enemyDetection = GetComponent<EnemyDetection>();

        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
        if (death)
        {
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
}
