using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerCube : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Player Touched Entity 
        if (collision.gameObject.CompareTag("Player")) collision.gameObject.GetComponent<PlayerController>().TakeDamage();

        //Projectile Hits Cube
        if (collision.gameObject.CompareTag("Player Projectile")) Destroy(collision.gameObject);
        if (collision.gameObject.CompareTag("Enemy Projectile")) Destroy(collision.gameObject);
    }
}
