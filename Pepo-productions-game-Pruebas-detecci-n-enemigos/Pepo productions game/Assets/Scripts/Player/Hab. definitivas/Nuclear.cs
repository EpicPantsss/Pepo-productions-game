using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuclear : MonoBehaviour
{
    private CircleCollider2D deathZone;

    private float timer;

    public float radiusEffect;
    private float radiusGrowth;

    void Start()
    {
        radiusGrowth = 0;

        deathZone = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10)
        {
            Destroy(this.gameObject);
        }

        if (radiusGrowth < radiusEffect)
        {
            radiusGrowth++;
            deathZone.radius = radiusGrowth;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
