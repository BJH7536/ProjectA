using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    public QuestManager quest;                //전체 패널에 붙이고 state를 채크해서 additem으로 지금 수행하고있는 퀘스트를 가져온다. 
    public Button[] QuestList;                //퀘스트 리스트 

    public TextMeshProUGUI QuestTitle;
    public TextMeshProUGUI QuestDescription;
}
