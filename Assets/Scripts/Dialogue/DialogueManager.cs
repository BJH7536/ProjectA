using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    private const int npc = 10000;
    private const int dialogue = 10;
    
    Dictionary<int, string[]> talkData;
    Dictionary<int,int> portraitData;
    Dictionary<int, string[]> ChoiceData;

    public static int Npc => npc;

    //퀘스트 진행상황은 퀘스트 메니저에서 관리


    private void Awake()
    {
        
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, int>();
        GenerateData();
    }

    void GenerateData()     //퀘스트번호+NPCId => 퀘스트용 대화 데이터 Id
    {
        var Textfile = @"c:/";
        if (File.Exists(Textfile))
        {
            using(var reader = new StreamReader(Textfile)) 
            { 
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] sentence = line.Split(",");
                }
            }
        }
        //Basic Talk Data
        talkData.Add(1000, new string[] { "안녕?:0", " 이 곳에는 어쩐일이야?:1" });

        talkData.Add(2000, new string[] { "안녕?:0", " 너는 누구야?:1" });

        //Quest Talk Data
        talkData.Add(1000+10, new string[] { "어서와:0", "옆의 npc2를 만나고올래?:1" });       //퀘스트 대화
        talkData.Add(1000 + 11, new string[] { "어:0", "npc를 만나고 왔어?:1" });                      //퀘스트 진행 중 대화
        talkData.Add(1000 + 12, new string[] { "옆의 npc2를 만나고 왔구나:1", "잘했어!:1" });                      //퀘스트 진행 중 대화
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
        if (!talkData.ContainsKey(id))       //퀘스트 진행중의 대사있는지 체크
        {
            //퀘스트 진행 중에는 퀘스트 진행 대사오기
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
