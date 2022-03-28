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
    public string[] leftAnimations = new string[4];
    public string[] rightAnimations = new string[4];
    public string[] upAnimations = new string[4];
    public string[] downAnimations = new string[4];

    private float timer;
    public float rotation;

    void Start()
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
        if (!animationStarted)
        {
            rotation = playerAttack.rotationAngle.z * 360;
            // Ratón a la izquierda
            if (rotation < 145 && rotation >= -145)
            {
                playerMovement.direction = PlayerMovement.Direction.LEFT;
                GetMovementAnimations(leftAnimations);
            }
            // Ratón abajo
            if (rotation > -345 && rotation <= -145)
            {
                playerMovement.direction = PlayerMovement.Direction.DOWN;
                GetMovementAnimations(downAnimations);
            }
            // Ratón a la derecha
            if (rotation < -345 || rotation >= 345)
            {
                playerMovement.direction = PlayerMovement.Direction.RIGHT;
                GetMovementAnimations(rightAnimations);
            }
            // Ratón arriba
            if (rotation < 345 && rotation >= 145)
            {
                GetMovementAnimations(upAnimations);
            }
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
                timer = 0;
            }
        }
    }
}
