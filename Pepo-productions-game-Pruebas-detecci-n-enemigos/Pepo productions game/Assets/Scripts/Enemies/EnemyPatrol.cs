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
            transform.Translate(Vector2.right * enemySpeed * Time.deltaTime);

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
        xDirection = objectives[patrolOrder].position.x - transform.position.x;
        yDirection = objectives[patrolOrder].position.y - transform.position.y;

        float distanceToRotate = getAngle(transform.position, objectives[patrolOrder].position);
        // Aplicas la rotación
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate - 0), 1);

        // Función que calcula cuánto necesitas rotar
        float getAngle(Vector2 position, Vector2 mousePosition)
        {
            float x = mousePosition.x - position.x;
            float y = mousePosition.y - position.y;

            return Mathf.Rad2Deg * Mathf.Atan2(y, x);
        }

        direction = new Vector2(xDirection, yDirection).normalized;
    }

    private void OnDrawGizmos()
    {
        // Para solo dibujar la ruta del enemigo cuando lo selecciones
        if (UnityEditor.Selection.activeGameObject != this.gameObject
            && UnityEditor.Selection.activeGameObject != this.gameObject.transform.GetChild(0).gameObject)
        {
            return;
        }

        Gizmos.color = Color.blue;
        for (int i = 0; i < objectives.Capacity - 1; i++)
        {
            Gizmos.DrawLine(objectives[i].position, objectives[i + 1].position);
        }
        Gizmos.DrawLine(objectives[objectives.Capacity - 1].position, objectives[0].position);
    }
}
