using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public bool isPaused;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }

        }
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }
    public void ExitToMain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(4);
    }
    
}
