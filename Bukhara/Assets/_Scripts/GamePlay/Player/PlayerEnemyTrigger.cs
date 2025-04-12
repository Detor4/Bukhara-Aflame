using UnityEngine;

namespace _Scripts.GamePlay.Player
{
    public class PlayerEnemyTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // Agar other obyektida Enemy script bo'lsa
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                // Obyekti yo'q qilish
                Destroy(other.gameObject);
            }
        }
    }
}