using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMecanism : MonoBehaviour
{
    public bool mecanismActivated;
    public Transform player;
    public GameObject mecanism;

    public float activationRange;

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < activationRange && Input.GetKeyDown(KeyCode.F))
        {
            mecanismActivated = true;
            mecanism.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        /*
        if (UnityEditor.Selection.activeGameObject != this.gameObject)
        {
            return;
        }

        Gizmos.DrawWireSphere(transform.position, activationRange);
        */
    }
}
