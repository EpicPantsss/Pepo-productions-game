using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [HideInInspector]
    public bool decreaseSpeed;

    private Rigidbody2D rb;

    private GameObject player;
    private PlayerAttack playerAttack;
    private PlayerDamage playerDamage;

    public Text text;
    public float startingKills = 0;
    public float currentKills = 0;  

    private bool hasSanguinary;

    private void Start()
    {
        enemyDetection = GetComponent<EnemyDetection>();

        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();


        player = GameObject.Find("Player");

        playerAttack = player.GetComponent<PlayerAttack>();
        playerDamage = player.GetComponent<PlayerDamage>();

        hasPatrolScript = enemyDetection.hasPatrolScript;

        currentKills = startingKills;
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
        if (decreaseSpeed)
        {
            timer += Time.deltaTime;
            if (timer > 1.5f)
            {
                decreaseSpeed = false;
                rb.velocity = Vector2.zero;
                timer = 0;
            }
        }
        if (death || enemyHP <= 0)
        {
            spriteRenderer.enabled = false;
            
            timer += Time.deltaTime;

            currentKills = +1;

            ScoreManager.instance.AddPoint();
            Destroy(gameObject);
            
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
                enemyHP -= playerAttack.bulletRepositoryScripts[playerAttack.actualBullet].bulletDamage;
            }
            if (enemyHP <= 0)
            {
                audioSource.clip = deathSound;
                audioSource.Play();
                spriteRenderer.enabled = false;
                death = true;
                ScoreManager.instance.AddPoint();
            }
            called = true;
            damaged = true;
        }
        if (other.CompareTag("MeleeWeapon"))
        {
            enemyHP -= playerAttack.bulletDamage;
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
        if (other.CompareTag("Infiltrado"))
        {
            GetComponent<StopEnemy>().stop = true;
        }

        if (other.CompareTag("Bullet"))
        {
            enemyHP--;
            if (enemyHP <= 0)
            {
                ScoreManager.instance.AddPoint();
                Destroy(gameObject);
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
        if (other.CompareTag("Infiltrado"))
        {
            GetComponent<StopEnemy>().stop = false;
        }
    }
}
