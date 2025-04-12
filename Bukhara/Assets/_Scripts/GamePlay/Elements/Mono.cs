using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Mono : MonoBehaviour
{
    public Transform kitob;
    public float height = 0.2f; // Qancha tepa-past harakat qiladi
    public float duration = 1f; // Qancha vaqtda harakat qiladi

    void Start()
    {
        HoverEffect();
    }

    void HoverEffect()
    {
        // Y tenglamasi bo‘yicha tepa-pastga harakat
        kitob.DOMoveY(kitob.position.y + height, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Action()
    {
        Destroy(gameObject);
    }
}