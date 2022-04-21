using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform Player;

    NavMeshAgent Enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        Enemy = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemy.SetDestination(Player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.gameObject.name);
        if (other.gameObject.name == "Player")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }

}
