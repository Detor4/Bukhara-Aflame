using System;
using _Scripts.GamePlay.Elements;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerBatotTrigger : MonoBehaviour
{
    private CharacterController controller;
    public float jumpForce = 10f; // qanchaga sakrash kerak

    private Vector3 jumpVelocity;
    private bool isJumping;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Agar sakrash kerak bo‘lsa, tepaga harakatlantiramiz
        if (isJumping)
        {
            controller.Move(jumpVelocity * Time.deltaTime);
            jumpVelocity += Physics.gravity * Time.deltaTime;

            // Yerga tushganini tekshirish (zamin bilan urildi)
            if (controller.isGrounded && jumpVelocity.y < 0)
            {
                isJumping = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Batot batot = other.GetComponent<Batot>();
        if (batot != null)
        {
            batot.Action();

            // Sakrashni boshlaymiz
            jumpVelocity = Vector3.up * jumpForce;
            isJumping = true;
        }
    }
}