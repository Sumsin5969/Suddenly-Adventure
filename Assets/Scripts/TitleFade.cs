using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleFade : MonoBehaviour
{
    
    public Image fadeImage;
    public float fadeDuration = 2.0f;
    void Start()
    {
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, fadeDuration, false);
    }

    public void LoadNextScene(string sceneName)
    {
         LoadSceneAfterFade(sceneName);
    }
    

    private IEnumerator LoadSceneAfterFade(string sceneName)
    {
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        SceneManager.LoadScene(sceneName);
        fadeImage.CrossFadeAlpha(0.0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }
}