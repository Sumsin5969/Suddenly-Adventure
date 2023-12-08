using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TypeEffect : MonoBehaviour
{
    TextProMeshUGUI msgText;
    public GameObject EndCursor;
    string targetMsg;
    public int CharPerSeconds;
    int index;
    private void Awake()
    {
        msgText = GetComponent<TextProMeshUGUI>();
    }
    public void SetMsg(string msg)
    {
        targetMsg = msg;
        EffectStart();
    }
    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false);
        
        Invoke("Effecting", 1 / CharPerSeconds);
    }
    void Effecting()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];
        index++;

        Invoke("Effecting", 1 / CharPerSeconds);
    }
    void EffectEnd()
    {
        EndCursor.SetActive(true);
    }
}
