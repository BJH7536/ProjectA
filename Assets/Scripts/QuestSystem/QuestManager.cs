using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int[] questId;               //전체 아이디를 보관하는게 좋을까?
    public int questActionIndex;
    public int questIndex;

    public Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        questIndex = 10;
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new MeetPeopleQuest("마을 사람 만나기", new int[] {1000,2000},10,QuestState.CAN_START,10));

        questList.Add(20, new CoincollectQuest("마을 사람 만나기", new int[] { 1000,2000}, 20, QuestState.CAN_START, 20));
    }

    public void AdvanceQuest(int id)            //퀘스트 진행상황 업데이트
    {
        if (questList[id].npcId.Length == 1)        //퀘스트에 연관된 npc가 한명일때
        {
            questActionIndex++;
            questList[questIndex].qs++;
            Debug.Log(questList[id].qs);
            if (questList[questIndex].qs == QuestState.FINISHED)
            {
                questActionIndex = 0;
                return;
            }
        }
        else//퀘스트에 연관된 npc가 둘이상이라면 npc 인덱스 늘려주기 -> 다음 npc와의 대화 가능하게
        {
            questActionIndex++;
            if (questActionIndex == questList[id].npcId.Length && questList[id].qs == QuestState.CAN_FINISH)     //퀘스트에 연관된 마지막 npc까지 만난 후 라면
            {
                questList[questIndex].qs++;
                AdvanceIndex();         //다음 퀘스트 진행 가능하게  
            }
            else if (questList[id].qs == QuestState.CAN_START)
            {
                questList[questIndex].qs++;
            }
            Debug.Log(questList[id].qs);
            if (questList[questIndex].qs == QuestState.FINISHED)
            {
                questActionIndex = 0;
                return;
            }
        }        

    }


    public void CheckRequirement(int index)             //진행 순서가 맞아진다면 시작가능한 퀘스트 체크
    {
        for (int i = 10; i < questList.Count; i += 10){
            if (index >= questList[i].Indexrequirment && (questList[i].qs!=QuestState.FINISHED || questList[i].qs!=QuestState.IN_PROGRESS))
            {
                questList[i].qs = QuestState.CAN_START;
                Debug.Log(questList[i].qs);
            }
        }
    }

    public QuestState CheckState(int id)          //퀘스트 진행가능 여부 판단
    {
        return questList[id].qs;
    }
    
    public void updateState(int id)
    {
        questList[id].updateQuest();
    }

    public int getnpcId(int id)
    {
        return questList[id].npcId[questActionIndex];
    }
    public int GetQuestTalkIndex(int id)            //NPC Id가 들어옴
    {
        return questIndex + questActionIndex;
    }

    public void AdvanceIndex()      //스토리 진행에 따라 다음 퀘스트가 진행될 수 있게 인덱스 값 증가
    {
        questIndex += 10;
        questActionIndex = 0;
    }

}
