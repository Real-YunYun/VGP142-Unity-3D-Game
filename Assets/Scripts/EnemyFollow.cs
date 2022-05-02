using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform Player;
    Animator Anim;
    public int Health { get; set; }

    NavMeshAgent Enemy;

    // Start is called before the first frame update
    void Start()
    {
        Health = 11;
        Enemy = this.gameObject.GetComponent<NavMeshAgent>();
        Anim = this.gameObject.GetComponent<Animator>();
        setRigidbodyState(true);
        setColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Health > 0)
        {
            Enemy.SetDestination(Player.position);
            Anim.SetFloat("MoveX", Enemy.velocity.x);
            Anim.SetFloat("MoveZ", Enemy.velocity.z);
        } else if (Health <= 0)
        {
            Death();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player") collision.gameObject.GetComponent<PlayerController>().TakeDamage(10);
    }

    private void Death()
    {
        Anim.enabled = false;
        Enemy.enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
    }

    public void TakeDamage(int value, string type = "death")
    {
        if (type == "death") Death();
        else if (type == "head") Anim.Play("Hit Head");
        else if (type == "kick") Anim.Play("Hit Rib");
        else if (type == "punch") Anim.Play("Hit Punch");

        Health -= value;

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
