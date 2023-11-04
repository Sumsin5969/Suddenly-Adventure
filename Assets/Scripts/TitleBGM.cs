using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBGM : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // 씬 전환 이벤트에 이벤트 핸들러 추가
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene unloadedScene)
    {
        // 씬이 언로드될 때 실행될 코드
        if (unloadedScene.name == "Title")
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Stop();
            Destroy(gameObject);
        }
    }
}