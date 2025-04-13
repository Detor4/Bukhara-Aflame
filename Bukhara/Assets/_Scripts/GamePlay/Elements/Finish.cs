using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private AudioSource winAudio;

    private bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
            finished = true;
            TriggerWin().Forget();
        }
    }

    private async UniTaskVoid TriggerWin()
    {
        if (winPanel != null)
        {
            // Avval panelni yo‘q qilamiz (hatto u Inspector'da true bo‘lsa ham)
            winPanel.transform.localScale = Vector3.zero;
            winPanel.SetActive(false);
        }

        // Audio o‘ynatish
        if (winAudio != null)
            winAudio.Play();

        // 2 soniya kutish
        await UniTask.Delay(TimeSpan.FromSeconds(2));

        // Panelni faollashtirib Dotween orqali chiqish animatsiyasi beramiz
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            winPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }
}