using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    public float cameraSpeed;

    public float rotation;

    private Vector2 mousePosition;
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 destiny = (mousePosition - (Vector2)target.position) / 5;

        float xdistanceToDestiny = (destiny.x - transform.position.x) + target.position.x;
        float ydistanceToDestiny = (destiny.y - transform.position.y) + target.position.y;

        transform.Translate(xdistanceToDestiny * cameraSpeed * Time.deltaTime, ydistanceToDestiny * cameraSpeed * Time.deltaTime, 0);
    }
}
