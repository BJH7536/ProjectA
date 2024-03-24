using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    public QuestManager quest;                //전체 패널에 붙이고 state를 채크해서 additem으로 지금 수행하고있는 퀘스트를 가져온다. 
    public Button[] QuestList;                //퀘스트 리스트 

    public TextMeshProUGUI QuestTitle;
    public TextMeshProUGUI QuestDescription;
    public ScrollRect QuestScrollRect;

    private void Start()
    {
        QuestScrollRect.normalizedPosition = new Vector2(1f, 1f);                   //사이즈 동적으로 바꿔주기

        Vector2 size = QuestScrollRect.content.sizeDelta;                   
        size.y = 5000f;
        QuestScrollRect.content.sizeDelta = size;
    }

    void addQuest(int id)
    {
        GameObject qeust= Resources.Load("QuestButton") as GameObject;

        GameObject instance = PrefabUtility.InstantiatePrefab(qeust) as GameObject;     //프리팹추가해주기
        instance.transform.SetParent(QuestScrollRect.content.transform);
    }
}
