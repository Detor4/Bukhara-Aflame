using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform target;          // 3D dunyodagi borishi kerak bo‘lgan joy
    public RectTransform arrowUI;    // UIdagi o‘qcha (arrow)
    public Camera mainCamera;        // Main kamera (Cinemachine bo‘lsa ham, bu yerga actual Camera kerak)

    void Update()
    {
        if (target == null) return;

        // 1. Target pozitsiyasini screen space (UI joylashuvi) ga o‘tkazish
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        // 2. Ekran orqasida bo‘lsa, arrowni yashirish
        if (screenPos.z < 0)
        {
            arrowUI.gameObject.SetActive(false);
            return;
        }

        arrowUI.gameObject.SetActive(true);

        // 3. Arrowni targetga qarating
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 dir = ((Vector2)screenPos - screenCenter).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrowUI.rotation = Quaternion.Euler(0, 0, angle);
    }
}