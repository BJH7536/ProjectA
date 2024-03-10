using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestData 
{
    [Header("Quest Info")]
    public string questName;
    public int[] npcId;     //퀘스트를 가지고있는 npcId\
    public int Indexrequirment;
    public QuestState qs;

    [Header("Reward Info")]
    public int goldReward;

    public enum QuestState
    {
        REQUIREMENTS_NOT_MET,
        CAN_START,
        IN_PROGRESS,
        FINISHED
    }

    public QuestData(string name, int[] npc,int Index,QuestState qs,int gold)
    {
        questName = name;
        npcId = npc;
        Indexrequirment=Index;
        qs = QuestState.REQUIREMENTS_NOT_MET;
        goldReward = gold;
    }

  
}
