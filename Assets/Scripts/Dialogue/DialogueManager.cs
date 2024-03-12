using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int,int> portraitData;
    Dictionary<int, string[]> ChoiceData;
    //����Ʈ �����Ȳ�� ����Ʈ �޴������� ����


    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, int>();
        GenerateData();
    }

    void GenerateData()     //����Ʈ��ȣ+NPCId => ����Ʈ�� ��ȭ ������ Id
    {
        //Basic Talk Data
        talkData.Add(1000, new string[] { "�ȳ�?:0", " �� ������ ��¾���̾�?:1" });

        talkData.Add(2000, new string[] { "�ȳ�?:0", " �ʴ� ������?:1" });

        //Quest Talk Data
        talkData.Add(1000+10, new string[] { "���:0", "���� npc2�� ������÷�?:1" });       //����Ʈ ��ȭ
        talkData.Add(1000 + 11, new string[] { "��:0", "npc�� ������ �Ծ�?:1" });                      //����Ʈ ���� �� ��ȭ
        talkData.Add(1000 + 12, new string[] { "���� npc2�� ������ �Ա���:1", "���߾�!:1" });                      //����Ʈ ���� �� ��ȭ
        talkData.Add(2000+11, new string[] { "�ȳ�?:0", "������ �Ϸ� ������� �Ծ�?:1" });
        portraitData.Add(1000 + 0, 0);
        portraitData.Add(1000 + 1, 1);
        portraitData.Add (1000 + 2, 2);

        portraitData.Add(2000 + 0, 0);
        portraitData.Add(2000 + 1, 1);
        portraitData.Add(2000 + 2, 2);
    }

    public string GetTalk(int id,int talkIndex)
    {
        if (!talkData.ContainsKey(id))       //����Ʈ �������� ����ִ��� üũ
        {
            //����Ʈ ���� �߿��� ����Ʈ ���� ������
            if (!talkData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id % 100, talkIndex);
            }
            else    //����Ʈ ��� ��������
            {
                return GetTalk(id - id % 10, talkIndex);

            }
        }
        //�⺻ ��� �������� �ڵ�
        if (talkIndex == talkData[id].Length) return null;
        else return talkData[id][talkIndex];
    }

    public int GetPortraitIndex(int id,int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }

}
