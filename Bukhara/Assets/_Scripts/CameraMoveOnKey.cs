using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraMoveOnKey : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float riseAmount = 5f;      // Necha birlik tepaga ko‘tarilsin
    public float duration = 5f;        // Qancha vaqt tursa
    public float moveDuration = 1f;    // Harakat davomiyligi

    private Vector3 originalOffset;
    private bool isMoving = false;

    void Start()
    {
        // Asl Follow Offset ni saqlab qo‘yamiz
        originalOffset = GetTransposer().m_FollowOffset;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isMoving)
        {
            isMoving = true;

            var transposer = GetTransposer();
            Vector3 targetOffset = originalOffset + new Vector3(0, riseAmount, 0);

            // Y o‘q bo‘yicha ko‘tarish
            DOTween.To(() => transposer.m_FollowOffset,
                       x => transposer.m_FollowOffset = x,
                       targetOffset,
                       moveDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                            {
                                // Turgandan keyin pastga tushish
                                DOVirtual.DelayedCall(duration, () =>
                                                                {
                                                                    DOTween.To(() => transposer.m_FollowOffset,
                                                                               x => transposer.m_FollowOffset = x,
                                                                               originalOffset,
                                                                               moveDuration)
                                                                        .SetEase(Ease.InSine)
                                                                        .OnComplete(() => isMoving = false);
                                                                });
                            });
        }
    }

    private CinemachineTransposer GetTransposer()
    {
        return virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }
}