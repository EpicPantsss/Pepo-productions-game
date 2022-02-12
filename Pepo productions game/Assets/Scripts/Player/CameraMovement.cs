using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    public float cameraSpeed;

    void Update()
    {
        float xDistance = target.position.x - transform.position.x;
        float yDistance = target.position.y - transform.position.y;

        transform.Translate(xDistance * cameraSpeed * Time.deltaTime, yDistance * cameraSpeed * Time.deltaTime, 0);
    }
}
