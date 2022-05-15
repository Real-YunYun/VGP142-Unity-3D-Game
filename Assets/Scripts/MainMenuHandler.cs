using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Forest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
