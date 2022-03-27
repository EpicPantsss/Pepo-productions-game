using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private enum AnimNames { STILL, RUN, RUNUP, RUNDOWN, ATTACK };

    private Animator anim;

    // Referencias a otros scripts
    private PlayerMovement playerMovement;             
    private PlayerAttack playerAttack;                 
                                                       
    private SpriteRenderer spriteRenderer;             
    private Rigidbody2D rb;                            
                                                       
    private bool animationStarted;                     
    [Header("Attack-4")]                               
    [Header("RunU-2 RunD-3")]
    [Header("Still-0 Run-1")]
    public List<string> animationsNames;
    public string[] leftAnimations = new string[3];
    public string[] rightAnimations = new string[3];
    public string[] upAnimations = new string[3];
    public string[] downAnimations = new string[3];

    private float timer;

    private bool running;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        playerMovement = GetComponent<PlayerMovement>();

        playerAttack = GetComponent<PlayerAttack>();
       
    }
    // Cambia las animaciones del jugador a aquellas que le indiques
    void GetMovementAnimations(string[] animNames)
    {
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
        else if (rb.velocity.y > 0.1f)
        {
            spriteRenderer.flipY = false;
            anim.Play(animNames[(int)AnimNames.RUNUP]);
        }
        else if (rb.velocity.y < -0.1f)
        {
            spriteRenderer.flipY = true;
            anim.Play(animNames[(int)AnimNames.RUNDOWN]);
        }
        else
        {
            anim.Play(animationsNames[(int)AnimNames.STILL]);
        }

    }
    void Update()
    {
        if (!animationStarted)
        {
            // Ratón a la derecha
            if (playerAttack.rotationAngle.z > 45 && playerAttack.rotationAngle.z <= 135)
                GetMovementAnimations(rightAnimations);
            // Ratón abajo
            if (playerAttack.rotationAngle.z > 135 && playerAttack.rotationAngle.z <= 225)
                GetMovementAnimations(upAnimations);
            // Ratón a la izquierda
            if (playerAttack.rotationAngle.z > 225 && playerAttack.rotationAngle.z <= 315)
                GetMovementAnimations(leftAnimations);
            // Ratón arriba
            if (playerAttack.rotationAngle.z > 315 && playerAttack.rotationAngle.z <= 45)
                GetMovementAnimations(leftAnimations);
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
