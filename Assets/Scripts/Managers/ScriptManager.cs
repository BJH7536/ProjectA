using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour 
{
    public GameObject talkPanel;
    public Text talkText;               //대화창이나 퀘스트창 내용
    public bool isActive;

    private void Awake()
    {
        talkPanel = GetComponent<GameObject>();
        talkText= GetComponentInChildren<Text>();
    }

    public void Talk(GameObject scanobj)
    {
        if(isActive)
        {
            isActive = false;
        }
        else
        {
            isActive = true;
            talkText.text="이것의 이름은 "+scanobj.name+"이라고 한다.";
        }

        talkPanel.SetActive(isActive);
    }

}
