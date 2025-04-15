using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraMoveOnKey : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float riseAmount = 5f;       // Necha birlik tepaga ko‘tarilsin
    public float moveDuration = 0.5f;   // Harakat davomiyligi

    private Vector3 originalOffset;
    private bool isUp = false;
    private Tween moveTween;

    void Start()
    {
        originalOffset = GetTransposer().m_FollowOffset;
    }

    void Update()
    {
        var transposer = GetTransposer();

        if (Input.GetKeyDown(KeyCode.Q) && !isUp)
        {
            isUp = true;
            Vector3 targetOffset = originalOffset + new Vector3(0, riseAmount, 0);

            // Oldingi animatsiyani to‘xtatamiz
            moveTween?.Kill();

            // Tepaga ko‘tarilishni boshlaydi
            moveTween = DOTween.To(() => transposer.m_FollowOffset,
                                   x => transposer.m_FollowOffset = x,
                                   targetOffset,
                                   moveDuration)
                .SetEase(Ease.OutSine);
        }

        if (Input.GetKeyUp(KeyCode.Q) && isUp)
        {
            isUp = false;

            // Oldingi animatsiyani to‘xtatamiz
            moveTween?.Kill();

            // Joyiga qaytadi
            moveTween = DOTween.To(() => transposer.m_FollowOffset,
                                   x => transposer.m_FollowOffset = x,
                                   originalOffset,
                                   moveDuration)
                .SetEase(Ease.InSine);
        }
    }

    private CinemachineTransposer GetTransposer()
    {
        return virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }
}