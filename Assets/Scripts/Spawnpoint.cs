using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{

    int randomSpawn;
    Transform defaultTransform;
    public Transform affectedPlayer;
    public Transform[] spawnpoints;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            if (!spawnpoints[0]) throw new UnassignedReferenceException("Reference to spawnpoints[] was Unassigned.");

            randomSpawn = UnityEngine.Random.Range(0, spawnpoints.Length - 1);

            if (affectedPlayer) affectedPlayer = spawnpoints[randomSpawn];
            else throw new NullReferenceException("Player Variable was not defined in Inspector.");

            if (randomSpawn < 0 || randomSpawn > spawnpoints.Length) throw new IndexOutOfRangeException("Attempted to access spawnpoint out of bounds of the array.");

            //DebugDisplay();

        } catch (UnassignedReferenceException e)
        {
            Debug.Log(e);
            affectedPlayer = defaultTransform;
            //DebugDisplay();

        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);

        } catch (IndexOutOfRangeException e)
        {
            Debug.Log(e);

        }
    }

    private void DebugDisplay()
    {
        Debug.Log("Player Spawned at: " + (randomSpawn + 1) +
        " X=" + spawnpoints[randomSpawn].transform.position.x +
        " Y=" + spawnpoints[randomSpawn].transform.position.y +
        " Z=" + spawnpoints[randomSpawn].transform.position.z
    );
    }
}
