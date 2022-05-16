using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") other.gameObject.GetComponent<PlayerController>().TakeDamage(5);
        if (other.gameObject.tag == "Enemy") other.gameObject.GetComponent<EnemyFollow>().TakeDamage(5, "head");
    }
}
