using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    public string sceneToLoad;
    private bool anyKeyPressed = false;

    void Update()
    {
        if (Input.anyKey && !anyKeyPressed) // 계속 다음 씬을 불러오는 오류를 방지
        {
            anyKeyPressed = true;
            TitleFade.Instance.LoadNextScene(sceneToLoad); // 스크립트 호출
        }
    }
}
