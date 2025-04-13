using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;

namespace _Scripts.GamePlay.Player
{
    public class PlayerEnemyTrigger : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private MonoBehaviour playerControllerScript;

        private bool isDead = false;

        private void OnTriggerEnter(Collider other)
        {
            if(isDead) return;

            Enemy enemyScript = other.GetComponent<Enemy>();

            if(enemyScript != null)
            {
                

                isDead = true;

                // Animatordagi Die true bo‘lsin
                if(animator != null)
                {
                    animator.SetBool("Die", true);
                    gameObject.tag = "PlayerMask"; // PlayerMask ga o‘zgartirish
                    gameObject.GetComponent<Collider>().enabled = false;
                }


                // PlayerController scriptni o‘chirish
                if(playerControllerScript != null)
                    playerControllerScript.enabled = false;

                // 5 soniyadan keyin GameOver panel ko‘rsatish
                ShowGameOverPanelDelayed().Forget();
            }
        }

        private async UniTaskVoid ShowGameOverPanelDelayed()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(5));

            if(gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }
    }
}