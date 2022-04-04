using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] Collider collider;
    public CharacterController CharController;

    public float MoveSpeed = 12f;
    public float Gravity = -10f;
    public float JumpHeight = 2.5f;

    public Transform GroundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 Velocity;
    bool isGrounded;

    private void Start()
    {
        collider = collider.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);

        if (isGrounded && Velocity.y < 0) Velocity.y = -0.2f;

        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");

        Vector3 Move = transform.right * MoveX + transform.forward * MoveZ;

        CharController.Move(Move * MoveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Velocity.y = Mathf.Sqrt(JumpHeight * -2 * Gravity);
        }

        Velocity.y += Gravity * Time.deltaTime;
        CharController.Move(Velocity * Time.deltaTime);
    }



}
