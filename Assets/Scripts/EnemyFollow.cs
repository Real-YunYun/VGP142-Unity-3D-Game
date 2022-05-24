using System.Collections; 
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public GameObject Player;
    public GameObject Drop;
    Animator Anim;
    [SerializeField] Transform origin;

    float rayRange = 1.0f;
    public float AggroDistance = 5f;
    public int StartHealth = 250;
    public int Health { get; set; }
    public Canvas EnemyCanvas;

    NavMeshAgent Enemy;

    Vector3 PatrolNode;
    private bool Patrolling = false;

    // Start is called before the first frame update
    void Start()
    {
        Health = StartHealth;
        Enemy = this.gameObject.GetComponent<NavMeshAgent>();
        Anim = this.gameObject.GetComponent<Animator>();
        setRigidbodyState(true);
        setColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.enabled)
        {
            ShowUI();
            float dist = Vector3.Distance(this.transform.position, Player.transform.position);

            if (dist <= AggroDistance && Health > 0)
            {
                Enemy.SetDestination(Player.transform.position);
                ShowUI(true);
            }
            else Patrol();
            Anim.SetFloat("MoveX", Enemy.velocity.x);
            Anim.SetFloat("MoveZ", Enemy.velocity.z);

            var RayCast = new Ray(this.gameObject.transform.position, transform.forward);
            var hits = Physics.RaycastAll(origin.position, origin.forward, rayRange, (byte)128);
            foreach (var hit in hits) if (hit.collider.tag == "Player") Anim.Play("Punch");
        }
        else ShowUI(true);
    }

    private void AttackRaycast(string AttackType)
    {
        var attack = Physics.RaycastAll(origin.position, origin.forward, rayRange);
        Debug.DrawRay(origin.position, origin.forward, Color.red);
        foreach (var hit in attack) if (hit.collider.tag == "Player") hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(10);
    }

    public void ResetAttack() {  }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player") collision.gameObject.GetComponent<PlayerController>().TakeDamage(10);
    }

    public void Death()
    {
        Health = 0;
        Enemy.enabled = false;
        Anim.enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        Destroy(gameObject, 5f);
        //I know this is the problem with the spawning but im leaving it cause it funny!
        Instantiate(Drop, this.transform.position, this.transform.rotation);
    }

    private void ShowUI(bool state = false)
    {
        EnemyCanvas.enabled = state;
        EnemyCanvas.transform.LookAt(Player.transform.position);
    }

    public void TakeDamage(int value, string type = "death")
    {
        Health -= value;
        if (type == "death" || Health <= 0) Death();
        else if (type == "head") Anim.Play("Hit Head");
        else if (type == "kick") Anim.Play("Hit Rib");
        else if (type == "punch") Anim.Play("Hit Punch");
        else if (type == "slash") Anim.Play("Hit Punch");

    }

    private void Patrol()
    {
        if (!Patrolling) StartCoroutine(RandomDestination());
    }

    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies) rigidbody.isKinematic = state;
        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders) collider.enabled = state;
        GetComponent<Collider>().enabled = !state;
    }

    IEnumerator RandomDestination()
    {
        if (!Patrolling)
        {
            Patrolling = true;
            PatrolNode = new Vector3(Random.Range(-10, 10) + this.transform.position.x, this.transform.position.y, Random.Range(-10, 10) + this.transform.position.z);
            Enemy.SetDestination(PatrolNode);
            yield return new WaitForSeconds(10f);
            Patrolling = false;
        }
    }
}
