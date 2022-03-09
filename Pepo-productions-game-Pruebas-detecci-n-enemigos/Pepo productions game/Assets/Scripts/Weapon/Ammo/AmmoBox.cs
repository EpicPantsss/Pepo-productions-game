using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class AmmoBox : MonoBehaviour
{
    public WeaponManager.AmmoTypes ammoType;

    private SpriteRenderer spriteRenderer;
    private Animator anim;

    public List<Sprite> spriteList;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //spriteRenderer.sprite = ;

        switch (ammoType)
        {
            case WeaponManager.AmmoTypes.PISTOL:
                spriteRenderer.sprite = spriteList[0];
                anim.SetInteger("Animation", 0);
                break;
            case WeaponManager.AmmoTypes.DEAGLE:
                spriteRenderer.sprite = spriteList[1];
                anim.SetInteger("Animation", -1);
                break;                             
            case WeaponManager.AmmoTypes.AK:       
                spriteRenderer.sprite = spriteList[2];
                anim.SetInteger("Animation", 1);
                break;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player") {
            Destroy(gameObject);
        }
    }
}
