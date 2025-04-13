using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource leftFootAudio;
    [SerializeField] private AudioSource rightFootAudio;
    [SerializeField] private float footstepInterval = 0.4f;

    private CharacterController controller;
    private Vector3 moveDirection;

    private float footstepTimer = 0f;
    private bool leftStepNext = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0, v).normalized;
        moveDirection = input * walkSpeed;

        controller.SimpleMove(moveDirection);
        RotateTowardsMovement(input);

        bool isWalking = input.magnitude > 0.1f;
        animator.SetBool("Walk", isWalking);

        if (isWalking)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                footstepTimer = 0f;

                if (leftStepNext)
                    leftFootAudio.Play();
                else
                    rightFootAudio.Play();

                leftStepNext = !leftStepNext;
            }
        }
        else
        {
            footstepTimer = footstepInterval; // reset timer
        }
    }

    void RotateTowardsMovement(Vector3 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }
}