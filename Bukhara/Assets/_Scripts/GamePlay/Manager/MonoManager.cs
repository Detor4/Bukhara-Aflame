using System;
using UnityEngine;
using UnityEngine.UI;

public class MonoManager : MonoBehaviour
{
    public static MonoManager Instance;

    public int MonoValue = 0;         // Hozirgi to‘plangan qiymat
    public int AllMonoValue = 2;     // Maksimal qiymat
    [SerializeField] private Image monoProgressBar; // Image - Fill Mode bo'lishi kerak

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateProgressBar();
    }

    public void AddMono()
    {
        MonoValue++;
        if (MonoValue > AllMonoValue) MonoValue = AllMonoValue;

        UpdateProgressBar();
        CheckMono();
    }

    private void UpdateProgressBar()
    {
        monoProgressBar.fillAmount = (float)MonoValue / AllMonoValue;
    }

    private void CheckMono()
    {
        if (MonoValue == AllMonoValue)
        {
            Debug.Log("All Mono Collected!");
        }
    }
}