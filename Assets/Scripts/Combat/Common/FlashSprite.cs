using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSprite : MonoBehaviour
{
    public void HitFlash(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer) StartCoroutine(DamageFlash(spriteRenderer));
    }

    IEnumerator DamageFlash(SpriteRenderer spriteRenderer)
    {
        // gets the sprite from either the object or the objects child if visuals are separate

        spriteRenderer.color = new Color(1, 0, 0, 0.5f);

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
