using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public bool Sword;
    public bool Bow;
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            if (Sword) other.gameObject.GetComponent<PlayerController>().ObtainedSword = Sword;
            if (Bow) other.gameObject.GetComponent<PlayerController>().ObtainedBow = Bow;
            Destroy(gameObject);
        } 
    }
}
