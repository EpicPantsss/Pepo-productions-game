using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(MeleeWeaponInfo))]
public class MeleeAttack : MonoBehaviour
{
    private BoxCollider2D triggerZone;

    public int damage;
    public float recoil;

    private float timer;

    private bool onRecoil;

    public string[] animNames = new string[2];

    private Animator anim;

    private PlayerAttack playerAttack;

    private bool animationStarted;
    [HideInInspector]
    public MeleeWeaponInfo meleeWeaponInfo;

    void Start()
    {
        meleeWeaponInfo = GetComponent<MeleeWeaponInfo>();
        triggerZone = GetComponent<BoxCollider2D>();
        triggerZone.enabled = false;

        anim = transform.parent.GetComponent<PlayerAnimations>().anim;
    }
    void Update()
    {
        if (onRecoil)
        {
            timer += Time.deltaTime;
            if (!animationStarted)
            {
                anim.Play(animNames[1]);
                animationStarted = true;
            }
            if (timer > recoil)
            {
                onRecoil = false;
                animationStarted = false;
                timer = 0;
            }
        }
    }
    public void Attack()
    {
        if (onRecoil) { return; }

        triggerZone.enabled = true;

        anim.Play(animNames[0]);

        onRecoil = true;
    }
}