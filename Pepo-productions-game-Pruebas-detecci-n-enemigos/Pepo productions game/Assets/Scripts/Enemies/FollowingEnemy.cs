using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : MonoBehaviour
{
    public float enemySpeed;

    private GameObject player;

    private float distanceToRotate;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        RaycastHit2D rayToPlayer = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        if (rayToPlayer)
        {
            distanceToRotate = getAngle(transform.position, player.transform.position);
            // =================
            // Rotaci�n del enemigo
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);

            float getAngle(Vector2 position, Vector2 mousePosition)
            {
                float x = mousePosition.x - position.x;
                float y = mousePosition.y - position.y;

                return Mathf.Rad2Deg * Mathf.Atan2(y, x);
            }
            // =================

            // Con esto el enemigo se mover� hacia adelante
            transform.Translate(Vector2.right * enemySpeed * Time.deltaTime);
        }
    }
}
