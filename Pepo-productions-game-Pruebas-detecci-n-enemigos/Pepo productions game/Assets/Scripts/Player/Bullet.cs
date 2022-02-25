using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage;

    public GameObject player;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = transform.parent.gameObject;

        transform.SetParent(null);

        StartMovement();
    }

    public void StartMovement()
    {
        transform.rotation = player.transform.rotation;
        transform.SetParent(null);
        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        StartCoroutine(WaitToReturn());
    }

    // Función para parar la bala y que vuelva al jugador
    private void ReturnToPlayer()
    {
        rb.velocity = new Vector2(0, 0);
        transform.SetParent(player.transform);
        transform.position = player.transform.position;
        this.gameObject.SetActive(false);
    }

    IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(1);
        ReturnToPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            ReturnToPlayer();
        }
    }
}
