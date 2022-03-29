using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class MeleeAttack : MonoBehaviour// COSAS POR MEJORAR ABAJO
{
    private BoxCollider2D triggerZone;

    private float recoil;

    private float timer;

    private bool onRecoil;

    private Animator anim;

    private PlayerAnimations playerAnimations;

    private bool animationStarted;


    public MeleeWeaponInfo meleeWeaponInfo;

    void Start()
    {
        playerAnimations = transform.parent.GetComponent<PlayerAnimations>();

        triggerZone = GetComponent<BoxCollider2D>();
        triggerZone.enabled = false;

        anim = playerAnimations.anim;
    }
    void Update()
    {
        if (onRecoil)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)// Desactivar la hitbox
                triggerZone.enabled = false;
            if (!animationStarted)// Empieza la animación de recuperación
            {
                anim.Play(meleeWeaponInfo.animationNames[1]);
                animationStarted = true;
            }
            if (timer > recoil)// Acaba el contador, y prepara las variables para poder volver a atacar
            {
                onRecoil = false;
                animationStarted = false;
                timer = 0;
            }
        }

        switch (playerAnimations.facingDirection)
        {
            case PlayerMovement.Direction.UP:
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.position = new Vector2(transform.parent.position.x, transform.parent.position.y + 1);
                break;
            case PlayerMovement.Direction.DOWN:
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.position = new Vector2(transform.parent.position.x, transform.parent.position.y - 1.7f);
                break;
            case PlayerMovement.Direction.RIGHT:
                transform.rotation = new Quaternion(0, 0, -1, -1);
                transform.position = new Vector2(transform.parent.position.x - 1, transform.parent.position.y);
                break;
            case PlayerMovement.Direction.LEFT:
                transform.rotation = new Quaternion(0, 0, 1, 1);
                transform.position = new Vector2(transform.parent.position.x + 1.7f, transform.parent.position.y);
                break;
        }
    }
    public void Attack()
    {
        if (onRecoil) { return; }

        triggerZone.enabled = true;

        anim.Play(meleeWeaponInfo.animationNames[0]);

        // Para mejorar, esto no se tendria que poner aquí
        recoil = meleeWeaponInfo.attackRecoil;

        onRecoil = true;
    }
}