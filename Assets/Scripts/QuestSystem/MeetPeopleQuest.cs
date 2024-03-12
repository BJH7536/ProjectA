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

    public override void updateQuest()
    {
        qs++;
    }
}
