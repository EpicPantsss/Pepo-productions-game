using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float bulletSpeed;

    private Transform player;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = transform.parent;

        transform.SetParent(null);

        StartMovement();
    }

    public void StartMovement()
    {
        transform.rotation = player.rotation;
        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        StartCoroutine(WaitToReturn());
    }

    IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(1);
        rb.velocity = new Vector2(0, 0);
        transform.SetParent(player);
        transform.position = player.position;
        this.gameObject.SetActive(false);
    }
}
