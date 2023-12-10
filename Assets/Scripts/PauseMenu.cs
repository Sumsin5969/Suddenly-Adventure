using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public bool IsPaused = false;
    public bool inOptionMenu = false;
    public GameObject pauseMenuCanvas;
    public GameObject optionCanvas;
    public GameObject BGM;


    void Start()
    {
        Instance = this;
        pauseMenuCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        // BGM이 할당되어 있지 않은 경우에 대한 예외 처리
        if (BGM == null)
        {
            Debug.LogError("BGM 변수에 할당된 게임 오브젝트가 없습니다. 인스펙터에서 설정해주세요.");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused == true && inOptionMenu == false)
            {
                Resume();
            }
            else if (IsPaused == false && inOptionMenu == false)
            {
                Pause();
            }
            if (inOptionMenu == true)
            {
                pauseMenuCanvas.SetActive(true);
                optionCanvas.SetActive(false);
                inOptionMenu = false;
            }
        }
    }
    public void Resume()
    {
        // BGM 오브젝트에서 AudioSource 컴포넌트를 가져옴
        AudioSource audioSource = BGM.GetComponent<AudioSource>();
        audioSource.volume = Mathf.Clamp01(audioSource.volume * 2.0f);
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
    public void Option()
    {
        Debug.Log("Option");
        if (inOptionMenu == false)
        {
            pauseMenuCanvas.SetActive(false);
            optionCanvas.SetActive(true);
            inOptionMenu = true;
        }
    }
    public void CloseOption()
    {
        pauseMenuCanvas.SetActive(true);
        optionCanvas.SetActive(false);
        inOptionMenu = false;
    }
    public void Pause()
    {
        // BGM 오브젝트에서 AudioSource 컴포넌트를 가져옴
        AudioSource audioSource = BGM.GetComponent<AudioSource>();
        audioSource.volume = Mathf.Clamp01(audioSource.volume * 0.5f);
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    public void Save()
    {
        SaveAndLoad.Instance.SaveDataToJson();
    }

    public void Load()
    {
        SaveAndLoad.Instance.LoadData();
    }
}
