using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Binary : Entity
{
    [Header("Biary Parameters")]
    [SerializeField] private GameObject ProjectilePrefab;
    private Transform ProjectileSpawn;
    private bool FireRateDelay = false;
    private NavMeshAgent Agent;
    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Player = GameManager.Instance.PlayerInstance.transform;
        ProjectileSpawn = transform.Find("Projectile Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (Agent.enabled)
        {
            Agent.SetDestination(Player.position);
            if (Agent.remainingDistance < 15f) Fire();
        }
    }

    public override void Death()
    {
        if (transform.parent)
        {
            Instantiate(base.DeathParticle, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
        else base.Death();
    }

    private void Fire() { if (!FireRateDelay) StartCoroutine("Delay"); }

    IEnumerator Delay()
    {
        FireRateDelay = true;
        Instantiate(ProjectilePrefab, ProjectileSpawn.position, ProjectileSpawn.rotation);
        yield return new WaitForSeconds(0.5f);
        FireRateDelay = false;
    }
}
