using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Image fadeImage;
    private float fadeDuration = 3.0f;
    private AudioSource musicAudioSource;
    public string sceneName;

    void Start()
    {
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, fadeDuration, false);
    }
    public void StartGame()
    {
        fadeImage.CrossFadeAlpha(1.0f, fadeDuration, false);
        StartCoroutine(LoadSceneAfterFade(sceneName));
    }
    private IEnumerator LoadSceneAfterFade(string sceneName)
    {
        // 페이딩 시간만큼 대기
        yield return new WaitForSeconds(fadeDuration + 1.0f);

        // 씬 로드
        SceneManager.LoadScene(sceneName);
    }
}