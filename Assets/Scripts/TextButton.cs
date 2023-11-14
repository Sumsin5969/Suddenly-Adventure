using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// 일시정지 메뉴에서 사용하는 버튼 이벤트 핸들러

public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI buttonText;

    void Start()
    {
        // Text 컴포넌트 가져오기
        buttonText = GetComponent<TextMeshProUGUI>();

        // 마우스 진입 이벤트
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointerEnterDelegate(); });

        // 마우스 나가기 이벤트
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnPointerExitDelegate(); });

        // 클릭 이벤트

        EventTrigger.Entry resumeClick = new EventTrigger.Entry();
        resumeClick.eventID = EventTriggerType.PointerClick;
        if (this.gameObject.name == "Resume")
        {
            resumeClick.callback.AddListener((data) => { OnResumeClickDelegate(); });
        }

        EventTrigger.Entry exitClick = new EventTrigger.Entry();
        exitClick.eventID = EventTriggerType.PointerClick;
        if (this.gameObject.name == "Exit")
        {
            exitClick.callback.AddListener((data) => { OnExitClickDelegate(); });
        }

        // 이벤트 트리거에 추가
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
        trigger.triggers.Add(resumeClick);
        trigger.triggers.Add(exitClick);
    }

    // 마우스 진입 이벤트 핸들러
    public void OnPointerEnterDelegate()
    {
        // 마우스가 텍스트에 진입했을 때 수행할 동작
        buttonText.fontSize = 90;
    }

    // 마우스 나가기 이벤트 핸들러
    public void OnPointerExitDelegate()
    {
        // 마우스가 텍스트에서 나갔을 때 수행할 동작
        buttonText.fontSize = 80;
    }

    // 클릭 이벤트 핸들러
    public void OnResumeClickDelegate()
    {
        // Resume 텍스트가 클릭되었을 때 수행할 동작
        Debug.Log("Resume 이벤트 발생");
        PauseMenu.Instance.Resume();
        buttonText.fontSize = 80;
    }
    public void OnExitClickDelegate()
    {
        // Exit 텍스트가 클릭되었을 때 수행할 동작
        Debug.Log("Exit 이벤트 발생");
        PauseMenu.Instance.Exit();
        buttonText.fontSize = 80;
    }

    // 마우스 진입 이벤트 핸들러
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterDelegate();
    }

    // 마우스 나가기 이벤트 핸들러
    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitDelegate();
    }

    // 클릭 이벤트 핸들러
    public void OnResumeClick(PointerEventData eventData)
    {
        OnResumeClickDelegate();
    }
    public void OnExitClick(PointerEventData eventData)
    {
        OnExitClickDelegate();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭 이벤트가 처리되면 이곳에 추가 로직을 작성하세요.
    }
}