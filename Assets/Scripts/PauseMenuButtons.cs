using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuButtons : MonoBehaviour
{
    public TextMeshProUGUI[] buttons;
    void Start()
    {
        // 모든 버튼에 대해 마우스 진입 및 나갈 시 이벤트 핸들러 등록
        foreach (TextMeshProUGUI button in buttons)
        {
            button.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entryMouseEnter = new EventTrigger.Entry();
            entryMouseEnter.eventID = EventTriggerType.PointerEnter;
            entryMouseEnter.callback.AddListener((data) => { OnMouseEnter(button); });
            button.GetComponent<EventTrigger>().triggers.Add(entryMouseEnter);

            EventTrigger.Entry entryMouseExit = new EventTrigger.Entry();
            entryMouseExit.eventID = EventTriggerType.PointerExit;
            entryMouseExit.callback.AddListener((data) => { OnMouseExit(button); });
            button.GetComponent<EventTrigger>().triggers.Add(entryMouseExit);
        }
        // 클릭시 이벤트는 따로 지정
        buttons[0].gameObject.AddComponent<Button>().onClick.AddListener(OnResumeClick);
        buttons[1].gameObject.AddComponent<Button>().onClick.AddListener(OnOptionClick);
        buttons[2].gameObject.AddComponent<Button>().onClick.AddListener(OnApplyClick);
        buttons[3].gameObject.AddComponent<Button>().onClick.AddListener(OnBackClick);
        buttons[4].gameObject.AddComponent<Button>().onClick.AddListener(OnExitClick);
        buttons[5].gameObject.AddComponent<Button>().onClick.AddListener(OnSaveClick);
        buttons[6].gameObject.AddComponent<Button>().onClick.AddListener(OnLoadClick);
    }
    void OnMouseEnter(TextMeshProUGUI button)
    {
        button.fontSize = 65;
    }
    void OnMouseExit(TextMeshProUGUI button)
    {
        button.fontSize = 60;
    }
    // 클릭 이벤트 추가하려면 아래 복붙해서 메서드만들고 위에 클릭이벤트리스너 추가하고
    // Suddenly씬 Pause메뉴 캔버스에 버튼 배열 하나 더 만들어서 참조할 tmp주가하면됨
    void OnResumeClick()
    {
        PauseMenu.Instance.Resume();
        buttons[0].fontSize = 60;
    }
    void OnOptionClick()
    {
        PauseMenu.Instance.Option();
        buttons[1].fontSize = 60;
    }
    void OnApplyClick()
    {
        VideoOption.Instance.ApplyBtnClick();
        buttons[2].fontSize = 60;
    }
    void OnBackClick()
    {
        PauseMenu.Instance.CloseOption();
        buttons[3].fontSize = 60;
    }
    void OnExitClick()
    {
        PauseMenu.Instance.Exit();
        buttons[4].fontSize = 60;
    }
    void OnSaveClick()
    {
        PauseMenu.Instance.Save();
        buttons[5].fontSize = 60;
    }

    void OnLoadClick()
    {
        PauseMenu.Instance.Load();
        buttons[6].fontSize = 60;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
