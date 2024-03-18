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
    public string loc;              //퀘스트npc위치

    [Header("Reward Info")]
    public int goldReward;
    private string name;
    private int[] npc;
    private int index;
    private int gold;

    public QuestData(string name, int[] npc,int Index,QuestState qs,int gold,string location)
    {
        questName = name;
        npcId = npc;
        Indexrequirment=Index;
        qs = QuestState.REQUIREMENTS_NOT_MET;
        goldReward = gold;
        loc=location;
    }

    protected QuestData(string name, int[] npc, int index, QuestState qs, int gold)
    {
        this.name = name;
        this.npc = npc;
        this.index = index;
        this.qs = qs;
        this.gold = gold;
    }

    public abstract void updateQuest();

    public abstract string getQuestInfo();
  
}
