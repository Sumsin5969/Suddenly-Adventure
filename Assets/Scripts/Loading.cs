using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loading : MonoBehaviour
{
    private bool anyKeyPressed = false;
    private AsyncOperation async; // 로딩
    public string SceneToLoad;
    public GameObject loadingIcon;
    public TextMeshProUGUI completionText;
    public float minAlpha = 0.05f;
    public float maxAlpha = 1.0f;
    private float currentAlpha = 1.0f;
    private bool isFadingOut = false;
    private float startTime;
    public float loadingDuration;
    private bool blinkingStarted = false;

    void Start ()
    {
        loadingIcon.gameObject.SetActive(true);
        completionText.gameObject.SetActive(false);
        startTime = Time.time; // 로딩 시작 시간 설정
        async = SceneManager.LoadSceneAsync(SceneToLoad); // 열고 싶은 씬
        async.allowSceneActivation = false;
    }
    void Update()
    {
        
        // 경과 시간을 체크하여 로딩이 5초 동안만 수행하도록 합니다.
        if (Time.time - startTime >= loadingDuration)
        {
            loadingIcon.gameObject.SetActive(false);
            completionText.gameObject.SetActive(true);
            if (!blinkingStarted)
            {
                StartCoroutine(BlinkText());
                blinkingStarted = true;
            }
            if (Input.anyKey && !anyKeyPressed)
            
            {
                SetCanOpen();
            }
        }
    }
    // 로딩 스크린에서 팁 등을 알려주는 경우. 버튼을 눌러 시작합니다 등을 표시
    public void SetCanOpen()
    {
        // async.allowSceneActivation을 true로 설정하여 씬을 열도록 허용
        async.allowSceneActivation = true;
    }
    private IEnumerator BlinkText()
    {
        while (true)
        {
            if (isFadingOut)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, minAlpha, Time.deltaTime * 2);
                if (currentAlpha <= minAlpha + 0.1f)
                {
                    isFadingOut = false;
                }
            }
            else
            {
                currentAlpha = Mathf.Lerp(currentAlpha, maxAlpha, Time.deltaTime * 2);
                if (currentAlpha >= maxAlpha - 0.02f)
                {
                    isFadingOut = true;
                }
            }

            completionText.alpha = currentAlpha;
            yield return null;
        }
    }
}

