using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public CharacterController CharController;
    public Animator BaseAnimator;

    public float MoveSpeed = 7f;
    public float Gravity = -10f;
    public float JumpHeight = 2.5f;

    public Transform GroundCheck;
    public float groundDistance = 0f;
    public LayerMask groundMask;

    Canvas canvas;

    Vector3 Velocity;
    bool isGrounded;
    public bool IsDying = false;
    public int Health { get; set; }
    private bool Invincible { get; set; }

    public Light flashlight;
    bool flashlightOn = false;

    void Start()
    {
        Health = 100;   
    }

    // Update is called once per frame
    void Update()
    {
        if (CharController.enabled)
        {

            if (Health <= 0) Death();
            isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);

            if (isGrounded && Velocity.y < .01f) Velocity.y = 0f;

            float MoveX = Input.GetAxis("Horizontal");
            float MoveZ = Input.GetAxis("Vertical");

            BaseAnimator.SetBool("IsGrounded", isGrounded);
            BaseAnimator.SetFloat("MoveX", MoveX);
            BaseAnimator.SetFloat("MoveY", Velocity.y);
            BaseAnimator.SetFloat("MoveZ", MoveZ);

            //Debug.Log("MoveX: " + MoveX + " MoveY: " + Velocity.y + " MoveZ: " + MoveZ);

            Vector3 Move = transform.right * MoveX + transform.forward * MoveZ;

            CharController.Move(Move * MoveSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Velocity.y = Mathf.Sqrt(JumpHeight * -2 * Gravity);
            }

            if (Input.GetKeyDown(KeyCode.Q)) Punch();
            if (Input.GetKeyDown(KeyCode.E)) Kick();

            if (Input.GetKeyDown(KeyCode.F) && flashlightOn)
            {
                flashlightOn = false;
                flashlight.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.F) && !flashlightOn)
            {
                flashlightOn = true;
                flashlight.enabled = true;
            }

            Velocity.y += Gravity * Time.deltaTime;
            CharController.Move(Velocity * Time.deltaTime);
        }
    }

    public void Punch() { BaseAnimator.Play("Punching"); }
    public void Kick() { BaseAnimator.Play("Kick"); }

    public void TakeDamage(int value)
    {
        if (!Invincible)
        {
            Health -= value;
            StartCoroutine(IFrames());
            Debug.Log("Damage Taken: " + value + " Health Remaining: " + Health);
        }
    }

    IEnumerator IFrames(float duration = 0.5f)
    {
        Invincible = true;
        if (duration > 0)
        {
            yield return new WaitForSeconds(duration);
            Invincible = false;
        }
        else if (duration == -1)
        {
            yield return new WaitForSeconds(3f);
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();

        }
    }
    public void Death()
    {
        /*
        setRigidbodyState(false);
        setColliderState(true);
        BaseAnimator.enabled = false;
        CharController.enabled = false;
         */
        IsDying = false;
        BaseAnimator.Play("Death Backward");
        StartCoroutine(IFrames(-1));        
    }

    void setRigidbodyState(bool state)
    {

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

}
