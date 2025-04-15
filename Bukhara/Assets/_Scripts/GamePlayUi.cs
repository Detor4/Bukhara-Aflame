using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayUi : MonoBehaviour
{
    [SerializeField] private GameObject gamePlayMenu;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            gamePlayMenu.SetActive(true);
        }
    }
    
    public void TiemScaleReset()
    {
        Time.timeScale = 1;
    }

    public void ResumeBtn()
    {
        Time.timeScale = 1;
        gamePlayMenu.SetActive(false);
    }
}
