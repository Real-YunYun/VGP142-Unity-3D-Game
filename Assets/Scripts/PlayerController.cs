using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    public Light flashlight;
    private Transform MainCamera;
    private CharacterController CharController;
    private Animator BaseAnimator;

    [Header("Weapon Prefab References")]
    public GameObject SwordPrefab;
    public GameObject ShieldPrefab;
    public GameObject BowPrefab;
    private Transform SwordTransform;
    private Transform ShieldBowTransform;

    [Header("Projectile Handler")]
    public GameObject ProjectilePrefab;
    private const float ProjectileSpeed = 1000f;

    //Movement Variables
    [Header("Movement Variables")]
    public float MoveSpeed = 3f;
    public float Gravity = -10f;
    public float JumpHeight = 1f;
    public bool isGrounded;
    public Vector3 Velocity;
    private float MoveX;
    private float MoveZ;

    [Header("Weapon Booleans")]
    public bool ObtainedSword = false;
    public bool ObtainedBow = false;
    private bool NoWeapon = true;
    private bool Sword = false;
    private bool Bow = false;

    [Header("Player States")]
    public bool IsAttacking = false;
    public bool IsDying = false;
    public int Health { get; set; }
    public int Coins { get; set; }
    private bool Invincible { get; set; }
    private bool MovementPowerUp { get; set; }

    [Header("Raycast Variables")]
    private float rayRange = 10.0f;
    private float attenuationRadius = 5.0f;
    private float AttackRange = 1f;

    void Start()
    {
        CharController = gameObject.GetComponent<CharacterController>();
        BaseAnimator = gameObject.GetComponent<Animator>();
        MainCamera = transform.Find("Main Camera").transform;
        
        SwordTransform = transform.Find("DummyRig/root/B-hips/B-spine/B-chest/B-upperChest/B-shoulder_R/B-upper_arm_R/B-forearm_R/B-hand_R/Sword Point").transform;
        ShieldBowTransform = transform.Find("DummyRig/root/B-hips/B-spine/B-chest/B-upperChest/B-shoulder_L/B-upper_arm_L/B-forearm_L/B-hand_L/Shield Point").transform;

        SwordPrefab.GetComponent<MeshRenderer>().enabled = false;
        ShieldPrefab.GetComponent<MeshRenderer>().enabled = false;
        BowPrefab.GetComponent<MeshRenderer>().enabled = false;
        Health = 100;
        Coins = 0;
        MovementPowerUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CharController.enabled)
        {
            if (Health <= 0) Death();
            isGrounded = Physics.CheckSphere(transform.position, 0.1f, (byte)8);
            if (isGrounded && Velocity.y < .01f) Velocity.y = 0f;
            ViewRaycast();
            if (!IsAttacking && isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon();
                if (Input.GetKeyDown(KeyCode.Mouse0) || (Input.GetKeyDown(KeyCode.Mouse1))) Attack();
                MoveX = Input.GetAxis("Horizontal");
                MoveZ = Input.GetAxis("Vertical");
                if (Input.GetButtonDown("Jump") && isGrounded) Velocity.y = Mathf.Sqrt(JumpHeight * -2 * Gravity);
            }

            BaseAnimator.SetBool("IsGrounded", isGrounded);
            BaseAnimator.SetFloat("MoveX", MoveX);
            BaseAnimator.SetFloat("MoveY", Velocity.y);
            BaseAnimator.SetFloat("MoveZ", MoveZ);

            Vector3 Move = transform.right * MoveX + transform.forward * MoveZ;
            CharController.Move(Move * MoveSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.F) && flashlight.enabled) flashlight.enabled = false;
            else if (Input.GetKeyDown(KeyCode.F) && !flashlight.enabled) flashlight.enabled = true;

            Velocity.y += Gravity * Time.deltaTime;
            CharController.Move(Velocity * Time.deltaTime);
        }
    }

    private void Attack()
    {
        IsAttacking = true;

        if (Input.GetKeyDown(KeyCode.Mouse0) && NoWeapon) { BaseAnimator.Play("Punching"); }
        if (Input.GetKeyDown(KeyCode.Mouse0) && Bow) { BaseAnimator.Play("Shooting Arrow"); }
        if (Input.GetKeyDown(KeyCode.Mouse0) && Sword) { BaseAnimator.Play("Sword Slash"); }
        if (Input.GetKeyDown(KeyCode.Mouse1))  { BaseAnimator.Play("Kick"); }

    }

    public void SpawnProjectie()
    {
        GameObject projectile = Instantiate(ProjectilePrefab, MainCamera.position, MainCamera.rotation);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, ProjectileSpeed));
    }

    public void ResetAttack() { IsAttacking = false; }

    private void ViewRaycast()
    {
        var RayCast = new Ray(MainCamera.position, transform.forward);
        var hits = Physics.SphereCastAll(RayCast, attenuationRadius, rayRange, (byte)32);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.DrawLine(RayCast.origin, hit.point, Color.yellow);
                hit.collider.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            }
        }
    }

    public void AttackRaycast(string AttackType)
    {
        IsAttacking = true;
        int AttackDamage = 10;

        if (AttackType == "punch") AttackDamage = 10;
        if (AttackType == "slash") AttackDamage = 35;
        if (AttackType == "kick") AttackDamage = 20;

        var attack = Physics.RaycastAll(MainCamera.position, MainCamera.forward, AttackRange, (byte)64);
        Debug.DrawRay(MainCamera.transform.position, (MainCamera.forward * AttackRange), Color.red, 1f);
        foreach (var hit in attack) if (hit.collider.tag == "Enemy") hit.collider.gameObject.GetComponent<EnemyFollow>().TakeDamage(AttackDamage, AttackType);
    }

    private void ChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            NoWeapon = true;
            Sword = false;
            Bow = false;
            SwordPrefab.GetComponent<MeshRenderer>().enabled = false;
            ShieldPrefab.GetComponent<MeshRenderer>().enabled = false;
            BowPrefab.GetComponent<MeshRenderer>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && ObtainedSword)
        {
            NoWeapon = false;
            Sword = true;
            Bow = false;
            SwordPrefab.GetComponent<MeshRenderer>().enabled = true;
            ShieldPrefab.GetComponent<MeshRenderer>().enabled = true;
            BowPrefab.GetComponent<MeshRenderer>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && ObtainedBow)
        {
            NoWeapon = false;
            Sword = false;
            Bow = true;
            SwordPrefab.GetComponent<MeshRenderer>().enabled = false;
            ShieldPrefab.GetComponent<MeshRenderer>().enabled = false;
            BowPrefab.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void TakeDamage(int value)
    {
        if (!Invincible)
        {
            Health -= value;
            StartCoroutine(IFrames());
        }
    }

    IEnumerator IFrames(float duration = 2f)
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
            SceneManager.LoadScene("GameOver Menu");
            /*
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
             */

        }
    }

    public void PowerUp(string type) { if (type == "Movement") StartCoroutine(ApplyPowerUp(type)); }

    IEnumerator ApplyPowerUp(string type, float duration = 5f)
    {
        MovementPowerUp = true;
        if (type == "Movement")
        {
            MoveSpeed += 3f;
            JumpHeight += 3f;
        }
        yield return new WaitForSeconds(duration);
        if (type == "Movement")
        {
            MoveSpeed -= 3f;
            JumpHeight -= 3f;
        }
        MovementPowerUp = false;
    }

    public void Death()
    {
        IsDying = true;
        setRigidbodyState(false);
        setColliderState(true);
        BaseAnimator.enabled = false;
        CharController.enabled = false;
        IsDying = false;
        //BaseAnimator.Play("Death Backward");
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
