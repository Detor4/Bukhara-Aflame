using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoManager : MonoBehaviour
{
    public static MonoManager Instance;

    public int MonoValue = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddMono()
    {
        MonoValue++;
        Debug.Log("MonoValue: " + MonoValue);
    }
}
