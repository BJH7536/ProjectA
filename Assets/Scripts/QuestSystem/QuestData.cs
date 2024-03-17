using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestData 
{
    [Header("Quest Info")]
    public string questName;
    public int[] npcId;     //����Ʈ�� �������ִ� npcId\
    public int Indexrequirment;
    public QuestState qs;

    [Header("Reward Info")]
    public int goldReward;



    public QuestData(string name, int[] npc,int Index,QuestState qs,int gold)
    {
        questName = name;
        npcId = npc;
        Indexrequirment=Index;
        qs = QuestState.REQUIREMENTS_NOT_MET;
        goldReward = gold;
    }

    public abstract void updateQuest();

    public abstract string getQuestInfo();
  
}
