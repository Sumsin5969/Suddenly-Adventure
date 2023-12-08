using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI talkText;
    public GameObject scanObject;
    public GameObject talkPanel;
    public bool isAction;
    // Start is called before the first frame update
    void Start()
    {
        talkPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 오브젝트 상호작용
    public void Action(GameObject scanObj)
    {
        if (isAction)
        {
            isAction = false;
        }
        else
        {
            isAction = true;
            scanObject = scanObj;
            talkText.text = "이것의 이름은 " + scanObject.name + " 이라고 한다.";
        }
        talkPanel.SetActive(isAction);
    }
}
