using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject enemy;

    public float visionAngle;
    public float aux;
    void Start()
    {
    }

    void Update()
    {
        Debug.DrawRay(transform.position, new Vector2(transform.position.x + visionAngle - aux, -transform.position.y + visionAngle), Color.blue);
        Debug.DrawRay(transform.position, new Vector2(-transform.position.y + visionAngle, transform.position.x + visionAngle + aux), Color.blue);

        Gizmos.DrawFrustum(transform.position, 5, 10, 1, 12);
    }
}
