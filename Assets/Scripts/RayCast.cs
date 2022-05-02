using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RayCast : MonoBehaviour
{
    [SerializeField] Transform origin;

    public string TargetTag;

    public float rayRange = 10.0f;
    public float attenuationRadius = 5.0f;
    public LayerMask Layer;


    float AttackRange = 2f;

    void Update()
    {
        var RayCast = new Ray(origin.transform.position, transform.forward);
        var AttackRayCast = new Ray(origin.transform.position, transform.forward);

        //Physics.Raycast(origin.transform.position, transform.forward, out hitInfo, rayRange, desiredLayers);

        var hits = Physics.SphereCastAll(RayCast, attenuationRadius, rayRange);
        var attack = Physics.Raycast(origin.position, origin.forward, AttackRange, Layer);

        Debug.DrawRay(origin.transform.position, (transform.forward * rayRange), Color.red);
        foreach (var hit in hits)
        {
            if (hit.collider.tag == TargetTag)
            {
                Debug.DrawLine(RayCast.origin, hit.point, Color.white);
                Debug.DrawRay(hit.point, hit.normal, Color.blue);

                Debug.DrawLine(RayCast.origin, hit.point, Color.green);
                hit.collider.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;     
                
                if (Input.GetKeyDown(KeyCode.Q) && attack)
                {
                    hit.collider.gameObject.GetComponent<EnemyFollow>().TakeDamage(10, "punch");
                } else if (Input.GetKeyDown(KeyCode.E) && attack)
                {
                    hit.collider.gameObject.GetComponent<EnemyFollow>().TakeDamage(10 , "kick");
                }
            }
        }
    }
}
