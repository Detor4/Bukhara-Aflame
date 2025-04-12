using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float sneakSpeed = 2f;
    [SerializeField] private Animator animator;


    private CharacterController controller;
    private Vector3 moveDirection;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Animator komponentini olamiz
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(h, 0, v);
        input = Vector3.ClampMagnitude(input, 1f);

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sneakSpeed : walkSpeed;
        moveDirection = input * currentSpeed;

        controller.SimpleMove(moveDirection);
        RotateTowardsMovement(input);

        // Animator: yurayotganini aniqlash
        bool isWalking = input.magnitude > 0.1f;
        animator.SetBool("Walk", isWalking);
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