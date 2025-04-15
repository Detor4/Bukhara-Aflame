using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public AudioSource audioSource;
    public AudioClip narrationClip;
    [TextArea]
    public string tutorialMessage;

    public Image[] images;
    public TextMeshProUGUI[] imageTexts; // 4 ta text, har biri image tagida chiqadi

    public string nextSceneName = "GamePlay";
    public string skipSceneName = "GamePlay";

    private bool isSkipped = false;

    async void Start()
    {
        // Boshlanishida barcha rasmlar va matnlarni o‘chirib qo‘yamiz
        foreach (var img in images)
        {
            img.gameObject.SetActive(false);
        }

        foreach (var txt in imageTexts)
        {
            txt.gameObject.SetActive(false);
        }

        await PlayIntroSequence();
    }

    async UniTask PlayIntroSequence()
    {
        // Text + Audio ni birga boshlash
        var textTask = TypeText(tutorialMessage);
        var audioTask = PlayAudio(narrationClip);

        await UniTask.WhenAll(textTask, audioTask);

        // Yozilgan textni yo'qotish
        mainText.DOFade(0, 1f);
        await UniTask.Delay(1000);

        // Image + text ketma-ket 5 soniyadan, keyin fade out
        for (int i = 0; i < images.Length; i++)
        {
            if (isSkipped) return;

            images[i].gameObject.SetActive(true);
            imageTexts[i].gameObject.SetActive(true);

            images[i].color = new Color(1, 1, 1, 0);
            imageTexts[i].color = new Color(1, 1, 1, 0);

            images[i].DOFade(1, 1f);
            imageTexts[i].DOFade(1, 1f);

            await UniTask.Delay(5000); // 5 soniya ko‘rsatiladi

            // Fade out qilish
            images[i].DOFade(0, 1f);
            imageTexts[i].DOFade(0, 1f);

            await UniTask.Delay(1000); // fade out tugashini kutish

            images[i].gameObject.SetActive(false);
            imageTexts[i].gameObject.SetActive(false);
        }

        if (!isSkipped)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    async UniTask TypeText(string fullText)
    {
        mainText.text = "";
        foreach (char c in fullText)
        {
            if (isSkipped) return;
            mainText.text += c;
            await UniTask.Delay(60);
        }
    }

    async UniTask PlayAudio(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.clip = clip;
        audioSource.Play();
        await UniTask.WaitUntil(() => !audioSource.isPlaying || isSkipped);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSkipped = true;
            SceneManager.LoadScene(skipSceneName);
        }
    }
}
