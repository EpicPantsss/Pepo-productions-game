using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public int bulletDamage;

    public GameObject player;
    private Rigidbody2D rb;
    [HideInInspector]
    public bool sanguinary;

    public bool enemyHitted;

    private PlayerDamage playerDamage;
    private PlayerAttack playerAttack;

    private GameObject passive;
    private Sanguinario sanguinario;

    public void InitBullet()
    {
        player = transform.parent.gameObject;

        playerAttack = player.GetComponent<PlayerAttack>();
        playerDamage = player.GetComponent<PlayerDamage>();
        // Habilidad pasiva
        passive = player.GetComponent<PlayerAttack>().passiveAbility;
        if (passive.TryGetComponent<Sanguinario>(out sanguinario))
            sanguinario = passive.GetComponent<Sanguinario>();


        rb = GetComponent<Rigidbody2D>();
    }

    public void StartMovement()
    {
        // Le asignamos la rotación a la bala
        transform.rotation = playerAttack.rotationAngle;
        transform.SetParent(null);
        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        StartCoroutine(WaitToReturn());
    }

    // Función para parar la bala y que vuelva al jugador
    public void ReturnToPlayer()
    {
        enemyHitted = false;
        /// Frenar la bala
        rb.velocity = new Vector2(0, 0);
        /// Poner la bala como hija del player, y poner esta en la posición de este
        transform.SetParent(player.transform);
        transform.position = player.transform.position;

        enemyHitted = false;

        this.gameObject.SetActive(false);
    }

    IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(1);
        ReturnToPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && sanguinary && !enemyHitted)
        {
            playerDamage.playerHP += sanguinario.healAmount;
            enemyHitted = true;
        }

        if (!other.CompareTag("Player") && !other.CompareTag("Aullador"))
        {
            ReturnToPlayer();
        }
    }
}
