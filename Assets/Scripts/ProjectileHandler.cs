using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyFollow>().TakeDamage(10, "head");
            Destroy(gameObject);
        }
        Destroy(gameObject, 2f);
    }
}
