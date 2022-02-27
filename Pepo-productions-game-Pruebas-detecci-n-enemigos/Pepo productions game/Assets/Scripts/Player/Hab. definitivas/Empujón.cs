using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empujón : MonoBehaviour
{
    private Rigidbody2D enemyRB;

    public float attackForce;

    private float timer;

    private GameObject playerGameObject;
    private PlayerMovement player;

    void Start()
    {
        playerGameObject = GameObject.Find("Player");    
    }
    void Update()
    {
        timer += Time.deltaTime;            
        if (timer > 5)
        {
            Destroy(this.gameObject);
        }
        player = playerGameObject.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(transform.rotation);
        if (!other.CompareTag("Player"))
        {
            GameObject objective;
            if (other.transform.parent != null)
            {
                objective = other.transform.parent.gameObject;
            }
            else
            {
                objective = other.gameObject;
            }

            objective.GetComponent<EnemyDetection>().knocked = true;

            enemyRB = objective.GetComponent<Rigidbody2D>();

            #region Cambiarle la rotación
            // Aquí calculas cuánto hay que rotar para que el objeto mire al mouse
            float distanceToRotate = getAngle(transform.position, objective.transform.position);
            // Aplicas la rotación
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);

            // Función que calcula cuánto necesitas rotar
            float getAngle(Vector2 position, Vector2 mousePosition)
            {
                float x = mousePosition.x - position.x;
                float y = mousePosition.y - position.y;

                return Mathf.Rad2Deg * Mathf.Atan2(y, x);
            }

            objective.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);
            #endregion

            enemyRB.AddForce(objective.transform.right * attackForce, ForceMode2D.Impulse);

            StartCoroutine(WaitTime(objective));
        }
    }

    IEnumerator WaitTime(GameObject objective)
    {
        objective.GetComponent<EnemyDetection>().knocked = false;
        yield return new WaitForSeconds(1f);
    }
}
