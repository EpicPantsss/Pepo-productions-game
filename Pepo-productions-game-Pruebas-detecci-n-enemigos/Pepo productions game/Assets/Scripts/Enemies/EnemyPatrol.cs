using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> objectives;

    public int patrolOrder;

    private Rigidbody2D rb;

    public float enemySpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolOrder = 0;
        SetDirection(objectives[patrolOrder]);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, objectives[patrolOrder].position) > 0)
        {
            transform.Translate(transform.right * enemySpeed * Time.deltaTime);
        }
        else
        {
            SetDirection(objectives[patrolOrder]);
        }
    }

    void SetDirection(Transform objective)
    {
        if (transform.position == objectives[patrolOrder].position)
        {
            if (patrolOrder < objectives.Capacity)
            {
                patrolOrder++;
            }
            else
            {
                patrolOrder = 0;
            }
        }
        float distanceToRotate = getAngle(transform.position, objectives[patrolOrder].position);

        // Aplicas la rotación
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);

        // Función que calcula cuánto necesitas rotar
        float getAngle(Vector2 position, Vector2 mousePosition)
        {
            float x = mousePosition.x - position.x;
            float y = mousePosition.y - position.y;

            return Mathf.Rad2Deg * Mathf.Atan2(y, x);
        }
    }
}
