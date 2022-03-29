using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;

    // Referencias a otros scripts
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;

    private bool animationStarted;

    private float timer;

    private bool aux = false;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        if (playerMovement.walking && !playerAttack.shooting && !animationStarted)
        {
            anim.Play("Player_walk");
        }
        else if (!playerAttack.shooting && !playerMovement.walking && !animationStarted)
        {
            anim.Play("Player_idle");
        }

        if (Input.GetKey(KeyCode.Mouse0) && !playerMovement.walking)
        {
            anim.Play(playerAttack.animations[0]);
            animationStarted = true;
        }
        else if (Input.GetKey(KeyCode.Mouse0) && playerMovement.walking && !animationStarted)
        {
            anim.Play(playerAttack.animations[1]);
            animationStarted = true;
            timer = 0;
        }

        if (animationStarted)
        {
            timer += Time.deltaTime;

            if (timer > 0.5f)
            {
                animationStarted = false;
                aux = false;
                timer = 0;
            }
        }
    }
}
