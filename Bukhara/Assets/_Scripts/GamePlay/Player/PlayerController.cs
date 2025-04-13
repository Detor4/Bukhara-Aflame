using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    [SerializeField] private Animator animator;


    private CharacterController controller;
    private Vector3 moveDirection;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0, v);
        input = Vector3.ClampMagnitude(input, 1); // Harakat yo'nalishini cheklash

        
        moveDirection = input * walkSpeed;

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