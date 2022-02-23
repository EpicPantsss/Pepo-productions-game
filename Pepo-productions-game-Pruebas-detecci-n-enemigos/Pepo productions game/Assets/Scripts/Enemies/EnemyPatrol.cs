using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetection))]
public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> objectives;

    public int patrolOrder;

    public float enemySpeed;

    public Vector2 direction;

    private float xDirection;
    private float yDirection;

    public bool playerSaw;

    private void Start()
    {
        GetDirection();
    }

    private void Update()
    {
        if (!playerSaw)
        {
            transform.Translate(transform.right * enemySpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, objectives[patrolOrder].position) <= 0.15f)
            {
                if (patrolOrder < objectives.Capacity - 1)
                {
                    patrolOrder++;
                }
                else
                {
                    patrolOrder = 0;
                }
                GetDirection();
            }
        }
    }

    public void GetDirection()
    {
        xDirection = Vector2.Angle(transform.position, objectives[patrolOrder].position);
        yDirection = objectives[patrolOrder].position.y - transform.position.y;

        direction = new Vector2(xDirection, yDirection).normalized;
    }

    public void ReturnToPatrol()
    {
        playerSaw = false;

        transform.rotation = new Quaternion(0,0, Vector2.Angle(transform.position, objectives[patrolOrder].position), Vector2.Angle(transform.position, objectives[patrolOrder].position));
    }
}
