using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoincollectQuest : QuestData
{
    private int coinsCollected = 0;
    private int coinsToComplete = 5;

    public CoincollectQuest(string name, int[] npc, int Index, QuestState qs, int gold, string location) : base(name, npc, Index, qs, gold, location)
    {
        base.questName = name;
        base.npcId = npc;
        base.Indexrequirment = Index;
        base.qs = qs;
        base.goldReward = gold;
        base.loc = location;
    }



    //코인을 수집함에 따라 퀘스트 진행상황 체크

    public void Getcoin(int coin)
    {
        coinsCollected++;
        updateQuest();
    }

    public override string getQuestInfo()
    {
        return "코인을 " + coinsToComplete.ToString() + "개 모아오세요";
    }

    public override void updateQuest()
    {
        if (coinsCollected == coinsToComplete)
        {
            qs++;
        }
    }
}
