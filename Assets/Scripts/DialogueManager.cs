using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TypeEffect talk;
    public GameObject scanObject;
    public GameObject talkPanel;
    public bool isAction;
    public int talkIndex;

    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenertateData();
    }
    // Start is called before the first frame update
    void Start()
    {
        talkPanel.SetActive(false);
    }
    // 오브젝트 상호작용
    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        talkPanel.SetActive(isAction);
    }
    void GenertateData()
    {
        talkData.Add(1000, new string[] { "왔구나, 김철수", "어딜 갔다가 이제 오는게냐?" });
        talkData.Add(2000, new string[] { "느낌이 좋지 않아..", "뭔가 큰일이 일어날 것 같은 기분이야" });
        talkData.Add(3000, new string[] { "대사를 입력해주세요." });
        talkData.Add(4000, new string[] { "대사를 입력해주세요." });
        talkData.Add(5000, new string[] { "대사를 입력해주세요." });
        talkData.Add(100, new string[] { "얼마 전, 갑자기 나타난 미궁 입구에 대한 내용이 적혀있다..." });
    }
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
    void Talk(int id, bool isNpc)
    {
        string talkData = "";
        if(talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            talkData = GetTalk(id, talkIndex);
        }
        if (talkData == null)
            {
                isAction = false;
                talkIndex = 0;
                return;
            }

        if (isNpc)
        {
            talk.SetMsg(talkData.Split(':')[0]);
        }
        else
        {
            talk.SetMsg(talkData);
        }

        isAction = true;
        talkIndex++;
    }
}
