using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonoManager : MonoBehaviour
{
    public static MonoManager Instance;

    public int MonoValue = 0;
    public int AllMonoValue;
    [SerializeField] private TMP_Text monoValueText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        monoValueText.text = "Books" + AllMonoValue;
    }

    public void AddMono()
    {
        MonoValue++;
        AllMonoValue--;
        monoValueText.text = "Books" + AllMonoValue;
        CheckMono();
    }

    private void CheckMono()
    {
        if(MonoValue == AllMonoValue)
        {
            Debug.Log("Mono value is 2");
        }
    }
}
