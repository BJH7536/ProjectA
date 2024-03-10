using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int[] questId;               //전체 아이디를 보관하는게 좋을까?
    public int questActionIndex;
    public int questIndex;

    Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        questIndex = 10;
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new CoincollectQuest("코인 모으기", new int[] {1000},10,QuestData.QuestState.CAN_START,10));

        questList.Add(10, new CoincollectQuest("코인 모으기", new int[] { 1000 }, 10, QuestData.QuestState.CAN_START, 10));
    }

    public void AdvanceQuest(int id)            //
    {
        if (questList[id].npcId.Length == 1)
        {
            questList[questIndex].qs++;
            Debug.Log(questList[id].qs);
            if (questList[questIndex].qs == QuestData.QuestState.FINISHED)
            {
                return;
            }
        }
        else//퀘스트에 연관된 npc가 둘이상이라면
        {
            questActionIndex++;
            if (questActionIndex == questList[id].npcId.Length)
            {
                AdvanceIndex();   
            }
            questList[questIndex].qs++;
            Debug.Log(questList[id].qs);
            if (questList[questIndex].qs == QuestData.QuestState.FINISHED)
            {
                return;
            }
        }        

    }


    public void CheckRequirement(int index)             //진행 순서가 맞아진다면 시작가능한 퀘스트 체크
    {
        for (int i = 10; i < questList.Count; i += 10){
            if (index >= questList[i].Indexrequirment && (questList[i].qs!=QuestData.QuestState.FINISHED || questList[i].qs!=QuestData.QuestState.IN_PROGRESS))
            {
                questList[i].qs = QuestData.QuestState.CAN_START;
                Debug.Log(questList[i].qs);
            }
        }
    }

    public bool CheckQuest(int id)          //퀘스트 진행가능 여부 판단
    {
        if (questList[id].qs==QuestData.QuestState.CAN_START)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetQuestTalkIndex(int id)            //NPC Id가 들어옴
    {
        return questIndex + questActionIndex;
    }

    public void AdvanceIndex()      //스토리 진행에 따라 퀘스트인덱스 값 늘려주기
    {
        questIndex += 10;
        questActionIndex = 0;
    }

    public bool FinishQuest(int id)
    {
        return true;
    }

}
