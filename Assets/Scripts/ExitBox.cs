using System.Collections; using System.Collections.Generic; using UnityEngine;

public class ExitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<PlayerController>().Health = 0;
        if (other.gameObject.CompareTag("Enemy")) other.gameObject.GetComponent<EnemyFollow>().Health = 0;
    }
}
