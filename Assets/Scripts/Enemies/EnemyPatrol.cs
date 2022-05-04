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
    [Header("Modos de patrulla")]
    public bool normalPatrol;
    public bool invertedReturn;
    private int actualDirection = 1;
    public bool randomPatrol;

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
    }

    private void ChangeObjective()
    {
        if (normalPatrol)
        {
            if (patrolOrder < objectives.Capacity - 1)
            {
                patrolOrder++;
            }
            else
            {
                patrolOrder = 0;
            }
        }
        else if (invertedReturn)
        {
            if (actualDirection == 1)
            {
                if (patrolOrder < objectives.Capacity - 1)
                {
                    patrolOrder++;
                }
                else
                {
                    actualDirection = -actualDirection;
                }
            }
            else
            {
                if (patrolOrder >= 1)
                {
                    patrolOrder--;
                }
                else
                {
                    patrolOrder = 0;
                }
                if (patrolOrder == 0)
                {
                    actualDirection = -actualDirection;
                }
            }
        }
        else
        {
            patrolOrder = Random.Range(0, objectives.Capacity);
        }
        GetDirection();
    }

    public void GetDirection()
    {
        nearObjective = false;

        distance = Vector2.Distance(transform.position, objectives[patrolOrder].position);
        nearDistance = distance / 2;

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
    }

    private void OnDrawGizmos()
    {
       
        // Para solo dibujar la ruta del enemigo cuando lo selecciones
        //if (UnityEditor.Selection.activeGameObject != this.gameObject
        //    && UnityEditor.Selection.activeGameObject != this.gameObject.transform.GetChild(0).gameObject)
        //{
        //    return;
        //}

        Gizmos.color = Color.blue;
        for (int i = 0; i < objectives.Capacity - 1; i++)
        {
            Gizmos.DrawLine(objectives[i].position, objectives[i + 1].position);
        }
        if (!invertedReturn)
            Gizmos.DrawLine(objectives[objectives.Capacity - 1].position, objectives[0].position);
        
    }
}
