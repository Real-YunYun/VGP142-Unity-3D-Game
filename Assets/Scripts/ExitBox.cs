using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
