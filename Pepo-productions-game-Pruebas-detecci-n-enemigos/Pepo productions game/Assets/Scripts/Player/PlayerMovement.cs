using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum Direction { UP, DOWN, LEFT, RIGHT };
    Direction direction;

    public float speed;

    public Animator anim;

    public bool walking;

    private Vector2 directionToRotate;

    private PlayerAttack playerAttack;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        walking = playerAttack.shooting;

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            direction = Direction.RIGHT;
            walking = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            direction = Direction.LEFT;
            walking = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            direction = Direction.UP;
            walking = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
            direction = Direction.DOWN;
            walking = true;
        }
        if (!Input.anyKey)
        {
            walking = false;
        }


        if (walking)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
}
