using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;

    public float visionAngle;
    public float aux;
    void Start()
    {
    }

    void Update()
    {
        /*
        Debug.DrawRay(transform.position, new Vector2(transform.position.x + visionAngle - aux, -transform.position.y + visionAngle), Color.blue);
        Debug.DrawRay(transform.position, new Vector2(-transform.position.y + visionAngle, transform.position.x + visionAngle + aux), Color.blue);

        RaycastHit2D rayToPlayer = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        */
    }
}
