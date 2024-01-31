using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public float delayInSeconds = 1f; // Adjust this value to set the delay

    public void StartGame()
    {
        Invoke("LoadStartScreen", delayInSeconds);
    }

    private void LoadStartScreen()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
