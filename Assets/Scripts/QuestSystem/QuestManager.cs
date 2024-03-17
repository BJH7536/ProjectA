using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int[] questId;               //��ü ���̵� �����ϴ°� ������?
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
        questList.Add(10, new MeetPeopleQuest("���� ��� ������", new int[] {1000,2000},10,QuestState.CAN_START,10));

        questList.Add(20, new CoincollectQuest("���� ��� ������", new int[] { 1000,2000}, 20, QuestState.CAN_START, 20));
    }

    public void AdvanceQuest(int id)            //����Ʈ �����Ȳ ������Ʈ
    {
        if (questList[id].npcId.Length == 1)        //����Ʈ�� ������ npc�� �Ѹ��϶�
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
        else//����Ʈ�� ������ npc�� ���̻��̶�� npc �ε��� �÷��ֱ� -> ���� npc���� ��ȭ �����ϰ�
        {
            questActionIndex++;
            if (questActionIndex == questList[id].npcId.Length && questList[id].qs == QuestState.CAN_FINISH)     //����Ʈ�� ������ ������ npc���� ���� �� ���
            {
                questList[questIndex].qs++;
                AdvanceIndex();         //���� ����Ʈ ���� �����ϰ�  
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


    public void CheckRequirement(int index)             //���� ������ �¾����ٸ� ���۰����� ����Ʈ üũ
    {
        for (int i = 10; i < questList.Count; i += 10){
            if (index >= questList[i].Indexrequirment && (questList[i].qs!=QuestState.FINISHED || questList[i].qs!=QuestState.IN_PROGRESS))
            {
                questList[i].qs = QuestState.CAN_START;
                Debug.Log(questList[i].qs);
            }
        }
    }

    public QuestState CheckState(int id)          //����Ʈ ���డ�� ���� �Ǵ�
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
    public int GetQuestTalkIndex(int id)            //NPC Id�� ����
    {
        return questIndex + questActionIndex;
    }

    public void AdvanceIndex()      //���丮 ���࿡ ���� ���� ����Ʈ�� ����� �� �ְ� �ε��� �� ����
    {
        questIndex += 10;
        questActionIndex = 0;
    }

}
