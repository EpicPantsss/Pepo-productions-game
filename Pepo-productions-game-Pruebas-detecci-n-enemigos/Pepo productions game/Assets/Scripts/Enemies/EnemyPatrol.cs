using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetection))]
public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> objectives;

    public int patrolOrder;
    public float distance;

    public float enemySpeed;


    public bool playerSaw;

    private bool nearObjective;
    public float nearDistance;

    public float rotateSpeed;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();

        GetDirection();
    }

    private void Update()
    {
        if (!playerSaw)
        {
            anim.SetBool("Walking", true);

            transform.Translate(Vector2.right * enemySpeed * Time.deltaTime);

            distance = Vector2.Distance(transform.position, objectives[patrolOrder].position);

            if (distance <= 0.15f)
            {
                ChangeObjective();
            }

            if (distance < nearDistance)
            {
                nearObjective = true;
            }
            else if (nearObjective && distance >= nearDistance)
            {
                ChangeObjective();
            }
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    private void ChangeObjective()
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

    public void GetDirection()
    {
        nearObjective = false;

        distance = Vector2.Distance(transform.position, objectives[patrolOrder].position);
        nearDistance = distance / 2;

        float distanceToRotate = getAngle(transform.position, objectives[patrolOrder].position);
        // Aplicas la rotaci�n
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate - 0), 1);

        // Funci�n que calcula cu�nto necesitas rotar
        float getAngle(Vector2 position, Vector2 mousePosition)
        {
            float x = mousePosition.x - position.x;
            float y = mousePosition.y - position.y;

            return Mathf.Rad2Deg * Mathf.Atan2(y, x);
        }
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