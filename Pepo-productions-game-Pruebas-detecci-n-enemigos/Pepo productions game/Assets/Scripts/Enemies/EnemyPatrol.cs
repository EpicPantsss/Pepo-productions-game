using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> objectives;

    public int patrolOrder;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolOrder = 0;
        SetDirection(objectives[patrolOrder]);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, objectives[patrolOrder].position) >= 0)
        {
            
        }
    }

    void SetDirection(Transform objective)
    {
        if (transform.position == objectives[patrolOrder].position)
        {
            patrolOrder++;
        }
        rb.MovePosition(objective.position);
        SetDirection(objectives[patrolOrder]);
    }
}
