using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool IsPaused = false;
    public GameObject pauseMenuCanvas;
    
    void Start(){
        pauseMenuCanvas.SetActive(false);
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused = true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume(){
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Pause(){
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }
}
