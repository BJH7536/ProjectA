using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoincollectQuest : QuestData
{
    private int coinsCollected = 0;
    private int coinsToComplete = 5;

    public CoincollectQuest(string name, int[] npc, int Index, QuestState qs, int gold) : base(name, npc, Index, qs, gold)
    {
        base.questName = name;
        base.npcId = npc;
        base.Indexrequirment = Index;
        base.qs = qs;
        base.goldReward = gold;
    }

    //������ �����Կ� ���� ����Ʈ �����Ȳ üũ

    public void Getcoin(int coin)
    {
        coinsCollected++;
        updateQuest();
    }

    public override string getQuestInfo()
    {
        return "������ " + coinsToComplete.ToString() + "�� ��ƿ�����";
    }

    public override void updateQuest()
    {
        if (coinsCollected == coinsToComplete)
        {
            qs++;
        }
    }
}
