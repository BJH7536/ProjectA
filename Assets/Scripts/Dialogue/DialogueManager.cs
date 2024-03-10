using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int,int> portraitData;
    Dictionary<int, string[]> ChoiceData;
    //퀘스트 진행상황은 퀘스트 메니저에서 관리


    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, int>();
        GenerateData();
    }

    void GenerateData()     //퀘스트번호+NPCId => 퀘스트용 대화 데이터 Id
    {
        //Basic Talk Data
        talkData.Add(1000, new string[] { "안녕?:0", " 이 곳에는 어쩐일이야?:1" });

        talkData.Add(2000, new string[] { "안녕?:0", " 너는 누구야?:1" });

        //Quest Talk Data
        talkData.Add(1000+10, new string[] { "어서와:0", " 오른쪽으로 가서 NPC를 만나봐!:1" });
        talkData.Add(1000 + 11, new string[] { "어:0", "NPC는 만나고왔어?:1" });
        talkData.Add(2000+11, new string[] { "안녕?:0", "무엇을 하러 여기까지 왔어?:1" });
        portraitData.Add(1000 + 0, 0);
        portraitData.Add(1000 + 1, 1);
        portraitData.Add (1000 + 2, 2);

        portraitData.Add(2000 + 0, 0);
        portraitData.Add(2000 + 1, 1);
        portraitData.Add(2000 + 2, 2);
    }

    public string GetTalk(int id,int talkIndex)
    {
        if (!talkData.ContainsKey(id))       //퀘스트 진행중의 대사
        {
            //퀘스트 대사없을 때는 기본 대사 가져오기
            if (!talkData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id % 100, talkIndex);
            }
            else    //퀘스트 대사 가져오기
            {
                return GetTalk(id - id % 10, talkIndex);

            }
        }
        //기본 대사 가져오기 코드
        if (talkIndex == talkData[id].Length) return null;
        else return talkData[id][talkIndex];
    }

    public int GetPortraitIndex(int id,int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }

}
