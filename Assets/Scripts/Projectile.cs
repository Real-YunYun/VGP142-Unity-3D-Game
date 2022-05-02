using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Prefab;
    public float ProjectileSpeed = 1000f;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject ball = Instantiate(Prefab, transform.position, transform.rotation);
            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, ProjectileSpeed));
        }
         
    }
}
