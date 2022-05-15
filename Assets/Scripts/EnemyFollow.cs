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
    public string TargetTag;

    float rayRange = 1.0f;
    float attenuationRadius = 0.25f;
    public LayerMask Layer;
    public float AggroDistance = 5f;
    public int Health { get; set; }
    public Canvas EnemyCanvas;

    NavMeshAgent Enemy;

    Vector3 PatrolNode;
    private bool Patrolling = false;

    // Start is called before the first frame update
    void Start()
    {
        Health = 250;
        Enemy = this.gameObject.GetComponent<NavMeshAgent>();
        Anim = this.gameObject.GetComponent<Animator>();
        setRigidbodyState(true);
        setColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0) Death();
        else
        {
            float dist = Vector3.Distance(this.transform.position, Player.transform.position);
            if (dist <= AggroDistance && Health > 0)
            {
                Enemy.SetDestination(Player.transform.position);
                ShowUI(true);
            }
            else
            {
                Patrol();
                ShowUI();
            }
            Anim.SetFloat("MoveX", Enemy.velocity.x);
            Anim.SetFloat("MoveZ", Enemy.velocity.z);

            var RayCast = new Ray(this.gameObject.transform.position, transform.forward);
            var hits = Physics.SphereCastAll(RayCast, attenuationRadius, rayRange);
            foreach (var hit in hits)
            {
                if (hit.collider.tag == TargetTag)
                {
                    Anim.Play("Punch");
                    Player.GetComponent<PlayerController>().TakeDamage(10);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player") collision.gameObject.GetComponent<PlayerController>().TakeDamage(10);
    }

    public void Death()
    {
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
