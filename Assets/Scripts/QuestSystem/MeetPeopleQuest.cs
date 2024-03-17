using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetPeopleQuest : QuestData
{
    public bool isMeet;

    public MeetPeopleQuest(string name, int[] npc, int Index, QuestState qs, int gold) : base(name, npc, Index, qs, gold)
    {
        base.questName = name;
        base.npcId = npc;
        base.qs = qs;
        base.goldReward = gold;
    }

    public override string getQuestInfo()
    {
        return npcId[1].ToString() + "을 만나고 오세요";
    }

    public override void updateQuest()
    {
        qs++;
    }
}
