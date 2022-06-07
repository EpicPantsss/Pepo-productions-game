using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private enum AnimNames { STILL, RUN, RUNUP, RUNDOWN, ATTACK };
    [HideInInspector]
    public Animator anim;

    // Referencias a otros scripts
    private PlayerMovement playerMovement;             
    private PlayerAttack playerAttack;                 
                                                       
    private SpriteRenderer spriteRenderer;             
    private Rigidbody2D rb;                            
                                                       
    private bool animationStarted;                     
    [Header("Attack-4")]
    [Header("RunU-2 RunD-3")]
    [Header("Still-0 Run-1")]

    private float timer;
    public float rotation;

    private bool stillNrunning;

    public PlayerMovement.Direction facingDirection;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        playerMovement = GetComponent<PlayerMovement>();

        playerAttack = GetComponent<PlayerAttack>();
       
    }
    // Cambia las animaciones del jugador a aquellas que le indiques
    void GetMovementAnimations(string[] animNames)
    {
        // x
        if (rb.velocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
            anim.Play(animNames[(int)AnimNames.RUN]);
        }
        else if (rb.velocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
            anim.Play(animNames[(int)AnimNames.RUN]);
        }
        // y
        else if (rb.velocity.y > 0.1f)
        {
            anim.Play(animNames[(int)AnimNames.RUNUP]);
        }
        else if (rb.velocity.y < -0.1f)
        {
            anim.Play(animNames[(int)AnimNames.RUNDOWN]);
        }
        else
        {
            spriteRenderer.flipX = false;
            anim.Play(animNames[(int)AnimNames.STILL]);
        }
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


        if (Input.GetKey(KeyCode.Mouse0))
        {
            spriteRenderer.flipX = false;
            animationStarted = true;

            if ((rb.velocity.x > 0.1f || rb.velocity.x < -0.1f)
                || (rb.velocity.y > 0.1f || rb.velocity.y < -0.1f))
            {
                anim.Play(playerAttack.animations[1]);
                stillNrunning = false;
            }
            else
            {
                anim.Play(playerAttack.animations[0]);
                stillNrunning = true;
            }
        }
        else
        {
            if (animationStarted)
            {
                timer += Time.deltaTime;

                if (timer > 0.3f || stillNrunning)
                {
                    animationStarted = false;
                    stillNrunning = false;
                    timer = 0;
                }
            }
        }
    }
}

