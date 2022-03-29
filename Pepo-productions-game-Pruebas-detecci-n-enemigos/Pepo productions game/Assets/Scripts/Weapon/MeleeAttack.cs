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
    private PlayerDamage playerDamage;
    private PlayerAttack playerAttack;

    private bool animationStarted;

    private Transform player;

    private bool hasSanguinary;

    public MeleeWeaponInfo meleeWeaponInfo;

    void Start()
    {
        player = transform.parent;

        playerAnimations = player.gameObject.GetComponent<PlayerAnimations>();
        playerDamage = player.gameObject.GetComponent<PlayerDamage>();
        playerAttack = player.gameObject.GetComponent<PlayerAttack>();

        if (playerAttack.passiveAbility.name == "Sanguinario")
            hasSanguinary = true;

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
                switch (playerAnimations.facingDirection)
                {
                    case PlayerMovement.Direction.LEFT:
                        anim.Play(meleeWeaponInfo.leftAnimations[1]);
                        break;
                    case PlayerMovement.Direction.UP:
                        anim.Play(playerAttack.upAnimations[0]);
                        break;
                    case PlayerMovement.Direction.RIGHT:
                        anim.Play(meleeWeaponInfo.downAnimations[1]);
                        break;
                    case PlayerMovement.Direction.DOWN:
                        anim.Play(meleeWeaponInfo.leftAnimations[1]);
                        break;
                }
                animationStarted = true;
            }
            if (timer > recoil)// Acaba el contador, y prepara las variables para poder volver a atacar
            {
                onRecoil = false;
                animationStarted = false;

                transform.SetParent(player);

                timer = 0;
            }
        }

        switch (playerAnimations.facingDirection)
        {
            case PlayerMovement.Direction.UP:
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.position = new Vector2(player.position.x, player.position.y + 1);
                break;
            case PlayerMovement.Direction.DOWN:
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.position = new Vector2(player.position.x, player.position.y - 1.7f);
                break;
            case PlayerMovement.Direction.LEFT:
                transform.rotation = new Quaternion(0, 0, -1, -1);
                transform.position = new Vector2(player.position.x - 1, player.position.y);
                break;
            case PlayerMovement.Direction.RIGHT:
                transform.rotation = new Quaternion(0, 0, 1, 1);
                transform.position = new Vector2(player.position.x + 1.7f, player.position.y);
                break;
        }
    }
    public void Attack()
    {
        if (onRecoil) { return; }
        // Activar la zona de ataque
        transform.SetParent(null);
        triggerZone.enabled = true;

        //anim.Play(meleeWeaponInfo.animationNames[0]);

        // Para mejorar, esto no se tendria que poner aquí
        recoil = meleeWeaponInfo.attackRecoil;

        onRecoil = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && hasSanguinary) {
            playerDamage.IncreaseHP(playerAttack.bulletDamage);
        }
    }
}