using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public CharacterController CharController;

    public float MoveSpeed = 7f;
    public float Gravity = -10f;
    public float JumpHeight = 2.5f;

    public Transform GroundCheck;
    public float groundDistance = 0f;
    public LayerMask groundMask;

    Vector3 Velocity;
    bool isGrounded;

    public Light flashlight;
    bool flashlightOn = false;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);

        if (isGrounded && Velocity.y < .01f) Velocity.y = 0f;

        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");

        Vector3 Move = transform.right * MoveX + transform.forward * MoveZ;

        CharController.Move(Move * MoveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Velocity.y = Mathf.Sqrt(JumpHeight * -2 * Gravity);
        }

        if (Input.GetKeyDown(KeyCode.Q) && flashlightOn)
        {
            flashlightOn = false;
            flashlight.enabled = false;
        } else if (Input.GetKeyDown(KeyCode.Q) && !flashlightOn)
        {
            flashlightOn = true;
            flashlight.enabled = true;
        }

        Velocity.y += Gravity * Time.deltaTime;
        CharController.Move(Velocity * Time.deltaTime);
    }

}
