using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBGM : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        // 씬 전환 이벤트에 이벤트 핸들러 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
    {
        // 씬이 로드될 때 실행될 코드
        if (loadedScene.name == "StartScreen")
        {
            // Title 씬에서 BGM 재생 시작
            audioSource.Play();
        }
    }

    private void OnSceneUnloaded(Scene unloadedScene)
    {
        // 씬이 언로드될 때 실행될 코드
        if (unloadedScene.name == "Loading")
        {
            audioSource.Stop();
        }
    }
}