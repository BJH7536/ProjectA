using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour
{
    private const int npcId = 10000;
    private const int dialogueId = 10;
    
    Dictionary<int, string[]> talkData;
    Dictionary<int,int> portraitData;
    Dictionary<int, string[]> ChoiceData;
    private NpcData npcdata;
    private Story currentStory;                                     //Ink 로 생성된 텍스트를 받아올 Class변수

    private const string SPEAKER_TAG = "speaker";                   //테그값들 테그값 : 변수
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private UI_DialoguePopup popup;

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

    //public void GetTalk2(int id, int talkIndex)
    //{
    //    currentStory = new Story();
    //}


    public int GetPortraitIndex(int id,int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }

    public void EnterDialogueMode(TextAsset inkJSON,NpcData npc)
    {
        currentStory = new Story(inkJSON.text);
        Player.GetInstance().isAction = true;
        popup.dialoguePanel.SetActive(true);
        npcdata = npc;
        //태그 초기화
        popup.displayNameText.text = "???";
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        Player.GetInstance().isAction= false;
        popup.dialoguePanel.SetActive(false);
        popup.dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)                   //더 보여줄 이야기가 있다면
        {
            popup.dialogueText.text = currentStory.Continue();            //한줄 출력
            DisplayChoices();                                       //선택이 있으면 선택출력
            //태그관리
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag parsed error : " + tag);
            }
            string tagkey = splitTag[0].Trim();
            string tagvalue = splitTag[1].Trim();

            switch (tagkey)
            {
                case SPEAKER_TAG:
                    popup.displayNameText.text = tagvalue;
                    break;
                case PORTRAIT_TAG:
                    popup.portraitImage.sprite = npcdata.npcPortrait[int.Parse(tagvalue)];
                    break;
                default:
                    Debug.LogWarning("Tag exists but not handled");
                    break;
            }

        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > popup.choices.Length)           //현재 선택지의 개수가 버튼의 개수보다 많으면 오류 
        {
            Debug.LogError("More choices than ever");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            popup.choices[index].gameObject.SetActive(true);
            popup.choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < popup.choices.Length; i++)
        {
            popup.choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(popup.choices[0].gameObject);
    }

    //public void MakeChoice(int choiceIndex, string npcId)
    //{
    //    currentStory.ChooseChoiceIndex(choiceIndex);
    //    Debug.Log(npcId);
    //    if (choiceIndex == 0 && npcId.Contains("Quest") && start)
    //    {
    //        //NPC id 와 Quest ID를 일치시켜야함
    //        Managers.Questevent.StartQuest(npcId);
    //    }
    //    else if (choiceIndex == 0 && npcId.Contains("Quest") && end)
    //    {
    //        Managers.Questevent.FinishQuest(npcId);
    //    }
    //}

}
