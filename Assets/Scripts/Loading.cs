using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private bool anyKeyPressed = false;
    private AsyncOperation async; // 로딩
    private bool canOpen = false;
    public string SceneToLoad;
    void Start ()
    {
        StartCoroutine("Load");
    }
    void Update()
    {
        if (Input.anyKey && !anyKeyPressed)
        {
            SetCanOpen();
        }
    }
    // 로딩 스크린에서 팁 등을 알려주는 경우. 버튼을 눌러 시작합니다 등을 표시
    // 필요없다면 canOpen을 처음부터 true로 초기화하면됨.
    public void SetCanOpen()
    {
        canOpen = true;
    }
    
    // 로딩
    IEnumerator Load()
    {
        async = SceneManager.LoadSceneAsync(SceneToLoad); // 열고 싶은 씬
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            Debug.Log(async.progress);

            yield return true;

            if (canOpen)
                async.allowSceneActivation = true;
        }
    }
}
