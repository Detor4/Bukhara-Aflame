using UnityEngine;
using TMPro;
using DG.Tweening;

public class StarterTutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // UI Text (TextMeshPro)
    public float fadeDuration = 0.5f;    // Fading uchun vaqt
    public float showDuration = 5f;      // Necha soniya ko‘rinadi
    public float moveAmount = 10f;       // Tebranish masofasi
    public float moveSpeed = 1f;         // Tebranish tezligi

    void Start()
    {
        if (tutorialText == null) return;

        // Boshlanishda textni yashirib qo‘yamiz
        tutorialText.alpha = 0;

        // Fading orqali chiqib keladi
        tutorialText.DOFade(1, fadeDuration).OnComplete(() =>
                                                        {
                                                            // Tebranish animatsiyasi (y o‘qi bo‘yicha yuqoriga va pastga)
                                                            tutorialText.transform.DOLocalMoveY(tutorialText.transform.localPosition.y + moveAmount, moveSpeed)
                                                                .SetLoops(-1, LoopType.Yoyo)
                                                                .SetEase(Ease.InOutSine);

                                                            // 5 soniyadan so‘ng yo‘qoladi
                                                            DOVirtual.DelayedCall(showDuration, () =>
                                                                                                {
                                                                                                    tutorialText.DOFade(0, fadeDuration).OnComplete(() =>
                                                                                                    {
                                                                                                        tutorialText.gameObject.SetActive(false);
                                                                                                    });
                                                                                                });
                                                        });
    }
}