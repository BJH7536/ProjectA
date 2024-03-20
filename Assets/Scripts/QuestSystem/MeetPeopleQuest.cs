using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetPeopleQuest : QuestData
{
    public bool isMeet;

    public MeetPeopleQuest(string name, int[] npc, int Index, QuestState qs, int gold, string location) : base(name, npc, Index, qs, gold, location)
    {
        base.questName = name;
        base.npcId = npc;
        base.Indexrequirment = Index;
        base.qs = qs;
        base.goldReward = gold;
        base.loc = location;
    }

    public override QuestData getQuestInfo()
    {
        return this;
    }

    public override void updateQuest()
    {
        qs++;
    }
}
