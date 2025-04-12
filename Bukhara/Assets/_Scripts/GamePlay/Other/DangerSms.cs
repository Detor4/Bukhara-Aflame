using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DangerSms : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    public void StartWarning()
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < 2; i++)
        {
            sequence.Append(spriteRenderer.DOFade(1f, 0.3f));
            sequence.Append(spriteRenderer.DOFade(0f, 0.3f));
        }

        // Oxiri 0 da qoladi
        sequence.OnComplete(() => spriteRenderer.color = new Color(
                                spriteRenderer.color.r,
                                spriteRenderer.color.g,
                                spriteRenderer.color.b,
                                0f
                            ));
    }
}