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

    void Update()
    {
        var RayCast = new Ray(origin.transform.position, transform.forward);
        //Physics.Raycast(origin.transform.position, transform.forward, out hitInfo, rayRange, desiredLayers);
        var hits = Physics.SphereCastAll(RayCast, attenuationRadius, rayRange);

        Debug.DrawRay(origin.transform.position, (transform.forward * rayRange), Color.red);
        foreach (var hit in hits)
        {
            if (hit.collider.tag == TargetTag)
            {
                Debug.DrawLine(RayCast.origin, hit.point, Color.white);
                Debug.DrawRay(hit.point, hit.normal, Color.blue);
                hit.collider.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

            }
        }
    }
}
