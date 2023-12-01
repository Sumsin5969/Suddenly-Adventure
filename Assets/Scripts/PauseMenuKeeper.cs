using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuKeeper : MonoBehaviour
{
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
    {
        // 씬이 로드될 때 실행될 코드
        if (loadedScene.name == "StartScreen" || loadedScene.name == "Title")
        {
            Destroy(gameObject);
        }
    }
}
