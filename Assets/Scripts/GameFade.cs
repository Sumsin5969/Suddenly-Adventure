using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFade : MonoBehaviour
{
    private AudioSource musicAudioSource;
    public string sceneName;
    public static GameFade Instance; // 다른 스크립트에서 편하게 불러오기 위한 인스턴스 생성

    public Image fadeImage;
    public float fadeDuration;

    private bool isFading = false;

    private void Awake()
    {
        // 이미 인스턴스가 존재하면 새로 생성하지 않음
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, fadeDuration, false);
    }

    public IEnumerator LoadNextScene(string sceneName)
    {
        if (!isFading)
        {
            isFading = true;
            fadeImage.CrossFadeAlpha(1.0f, fadeDuration, false);
            yield return new WaitForSeconds(fadeDuration);
            SceneManager.LoadScene(sceneName);
            fadeImage.CrossFadeAlpha(0.0f, fadeDuration, false);
            yield return new WaitForSeconds(fadeDuration);
            isFading = false;   
        }
    }

    
}