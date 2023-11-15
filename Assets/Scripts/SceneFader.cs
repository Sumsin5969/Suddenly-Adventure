using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;
    public Image fadeImage;
    private float fadeDuration = 1.0f;
    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(FadeIn());
    }

    private void FadeToScene()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }
    
    private IEnumerator FadeOut()
    {
        fadeImage.CrossFadeAlpha(1.0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }
}