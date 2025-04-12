using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float sneakSpeed = 2f;

    private CharacterController controller;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(h, 0, v);
        input = Vector3.ClampMagnitude(input, 1f); // Diagonal yurishni tezlashtirmaslik uchun

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sneakSpeed : walkSpeed;
        moveDirection = input * currentSpeed;

        controller.SimpleMove(moveDirection);
        RotateTowardsMovement(input);
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