using UnityEngine;
public class Coin : MonoBehaviour
{
    private int RotationSpeed = 4;
    void Update()
    {
        transform.Rotate(new Vector3(0, Mathf.Sin(Time.time) * RotationSpeed, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().Coins += 1;
            Destroy(gameObject);
        }
    }

}
