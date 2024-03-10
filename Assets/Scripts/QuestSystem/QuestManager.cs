using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int[] questId;               //��ü ���̵� �����ϴ°� ������?
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
        questList.Add(10, new CoincollectQuest("���� ������", new int[] {1000},10,QuestData.QuestState.CAN_START,10));

        questList.Add(10, new CoincollectQuest("���� ������", new int[] { 1000 }, 10, QuestData.QuestState.CAN_START, 10));
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
        else//����Ʈ�� ������ npc�� ���̻��̶��
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


    public void CheckRequirement(int index)             //���� ������ �¾����ٸ� ���۰����� ����Ʈ üũ
    {
        for (int i = 10; i < questList.Count; i += 10){
            if (index >= questList[i].Indexrequirment && (questList[i].qs!=QuestData.QuestState.FINISHED || questList[i].qs!=QuestData.QuestState.IN_PROGRESS))
            {
                questList[i].qs = QuestData.QuestState.CAN_START;
                Debug.Log(questList[i].qs);
            }
        }
    }

    public bool CheckQuest(int id)          //����Ʈ ���డ�� ���� �Ǵ�
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

    public int GetQuestTalkIndex(int id)            //NPC Id�� ����
    {
        return questIndex + questActionIndex;
    }

    public void AdvanceIndex()      //���丮 ���࿡ ���� ����Ʈ�ε��� �� �÷��ֱ�
    {
        questIndex += 10;
        questActionIndex = 0;
    }

    public bool FinishQuest(int id)
    {
        return true;
    }

}
