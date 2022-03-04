using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    public float cameraSpeed;
    public float cameraToMouseSpeed;

    private Vector2 mousePosition, destiny;
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destiny = (mousePosition - (Vector2)target.position) / 8;

        float xdistanceToDestiny = (destiny.x - transform.position.x) + target.position.x;
        float ydistanceToDestiny = (destiny.y - transform.position.y) + target.position.y;

        float xDistancetoTarget = (target.position.x - transform.position.x) / 5;
        float yDistancetoTarget = (target.position.y - transform.position.y) / 5;

        transform.Translate((xdistanceToDestiny * cameraToMouseSpeed) * Time.deltaTime, 
                            (ydistanceToDestiny * cameraToMouseSpeed) * Time.deltaTime, 
                            0);
        transform.Translate((xDistancetoTarget * cameraSpeed) * Time.deltaTime, 
                            (yDistancetoTarget * cameraSpeed) * Time.deltaTime,
                            0);
    }
}
