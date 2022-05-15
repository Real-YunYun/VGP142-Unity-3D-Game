using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public string Type;
    Collider thisCollider;

    // Start is called before the first frame update
    void Start()
    {
        thisCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().PowerUp(Type);
            Destroy(this.gameObject);
        }
    }
}
