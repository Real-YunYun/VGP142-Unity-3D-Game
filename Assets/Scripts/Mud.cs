using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{    
    public float SlowSpeed = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") other.gameObject.GetComponent<PlayerController>().MoveSpeed /= 2f;
        if (other.gameObject.tag == "Player") other.gameObject.GetComponent<PlayerController>().JumpHeight /= 3f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") other.gameObject.GetComponent<PlayerController>().MoveSpeed *= 2f;
        if (other.gameObject.tag == "Player") other.gameObject.GetComponent<PlayerController>().JumpHeight *= 3f;
    }
}
