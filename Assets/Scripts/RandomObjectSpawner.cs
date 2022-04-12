using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] RandomSpawnList;

    // Start is called before the first frame update
    void Start()
    {
        int randomInt = Mathf.RoundToInt(Random.Range(0, RandomSpawnList.Length - 1));
        Transform t = gameObject.GetComponent<Transform>();
        Instantiate(RandomSpawnList[randomInt], t.position, t.rotation);
        Debug.Log("Spawned: " + randomInt);
    }
}
