using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public float fadeDuration;
    private void StartGame()
    {
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        GameFade.Instance.LoadNextScene("Suddenly");
    }
}
