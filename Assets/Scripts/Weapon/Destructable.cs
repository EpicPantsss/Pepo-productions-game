using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.tag == "object" || collision.tag == "Enemy")
        {
            Destroy(gameObject);
        }     
    }



}
