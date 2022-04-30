using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infiltrado : MonoBehaviour
{
    private CircleCollider2D triggerZone;

    private float timer;

    public float radiusEffect;
    private float radiusGrowth;

    public float infiltrateTime;

    private bool decrease;

    void Start()
    {
        radiusGrowth = 0;

        triggerZone = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > infiltrateTime)
        {
            decrease = true;
        }

        if (radiusGrowth < radiusEffect && !decrease)
        {
            radiusGrowth++;
            triggerZone.radius = radiusGrowth;
        }
        if (decrease)
        {
            radiusGrowth--;
            if (radiusGrowth <= 1.05f)
                Destroy(gameObject);
            triggerZone.radius = radiusGrowth;
        }
    }
}
