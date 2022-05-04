using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoofs : MonoBehaviour
{
    private bool hide;
    private bool startProcess;
    private SpriteRenderer spriteRenderer;

    float alpha;

    public float appearSpeed;

    private void Update()
    {
        if (startProcess)
        {
            if (hide)
            {
                if (alpha > 0)
                    alpha -= Time.deltaTime * 0.1f * appearSpeed;

                spriteRenderer.color = new Color(spriteRenderer.color.r,
                                                      spriteRenderer.color.g,
                                                      spriteRenderer.color.b,
                                                      alpha);
            }
            else
            {
                if (alpha < 1)
                    alpha += Time.deltaTime * 0.1f * appearSpeed;
                if (alpha >= 1)
                    startProcess = false;

                spriteRenderer.color = new Color(spriteRenderer.color.r,
                                                      spriteRenderer.color.g,
                                                      spriteRenderer.color.b,
                                                      alpha);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Roof"))
        {
            spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
            alpha = spriteRenderer.color.a;
            hide = true;
            startProcess = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Roof"))
        {
            alpha = spriteRenderer.color.a;
            hide = false;
        }
    }
}
