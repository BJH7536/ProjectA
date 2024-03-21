using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour
{
    private const int npc = 10000;
    private const int dialogue = 10;
    
    Dictionary<int, string[]> talkData;
    Dictionary<int,int> portraitData;
    Dictionary<int, string[]> ChoiceData;
    public static int Npc => npc;
    private Story currentStory;                                     //Ink �� ������ �ؽ�Ʈ�� �޾ƿ� Class����

    //����Ʈ �����Ȳ�� ����Ʈ �޴������� ����


    private void Awake()
    {
        
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, int>();
        GenerateData();
    }

    void GenerateData()     //����Ʈ��ȣ+NPCId => ����Ʈ�� ��ȭ ������ Id
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

    //public void GetTalk2(int id, int talkIndex)
    //{
    //    currentStory = new Story();
    //}


    public int GetPortraitIndex(int id,int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }

    //public void EnterDialogueMode(TextAsset inkJSON)
    //{
    //    currentStory = new Story(inkJSON.text);
    //    dialogueIsPlaying = true;
    //    dialoguePanel.SetActive(true);

    //    //�±� �ʱ�ȭ
    //    displayNameText.text = "???";
    //    portraitAnimator.Play("default");
    //    layoutAnimator.Play("right");

    //    ContinueStory();
    //}

    //private void ExitDialogueMode()
    //{
    //    dialogueIsPlaying = false;
    //    dialoguePanel.SetActive(false);
    //    dialogueText.text = "";
    //}

    //private void ContinueStory()
    //{
    //    if (currentStory.canContinue)                   //�� ������ �̾߱Ⱑ �ִٸ�
    //    {
    //        dialogueText.text = currentStory.Continue();            //���� ���
    //        DisplayChoices();                                       //������ ������ �������
    //        //�±װ���
    //        HandleTags(currentStory.currentTags);
    //    }
    //    else
    //    {
    //        ExitDialogueMode();
    //    }
    //}

    //private void HandleTags(List<string> currentTags)
    //{
    //    foreach (string tag in currentTags)
    //    {
    //        string[] splitTag = tag.Split(':');
    //        if (splitTag.Length != 2)
    //        {
    //            Debug.LogError("Tag parsed error : " + tag);
    //        }
    //        string tagkey = splitTag[0].Trim();
    //        string tagvalue = splitTag[1].Trim();

    //        switch (tagkey)
    //        {
    //            case SPEAKER_TAG:
    //                displayNameText.text = tagvalue;
    //                break;
    //            case PORTRAIT_TAG:
    //                portraitAnimator.Play(tagvalue);
    //                break;
    //            case LAYOUT_TAG:
    //                layoutAnimator.Play(tagvalue);
    //                break;
    //            default:
    //                Debug.LogWarning("Tag exists but not handled");
    //                break;
    //        }

    //    }
    //}

    //private void DisplayChoices()
    //{
    //    List<Choice> currentChoices = currentStory.currentChoices;

    //    if (currentChoices.Count > choices.Length)           //���� �������� ������ ��ư�� �������� ������ ���� 
    //    {
    //        Debug.LogError("More choices than ever");
    //    }

    //    int index = 0;
    //    foreach (Choice choice in currentChoices)
    //    {
    //        choices[index].gameObject.SetActive(true);
    //        choicesText[index].text = choice.text;
    //        index++;
    //    }

    //    for (int i = index; i < choices.Length; i++)
    //    {
    //        choices[i].gameObject.SetActive(false);
    //    }

    //    StartCoroutine(SelectFirstChoice());
    //}

    //private IEnumerator SelectFirstChoice()
    //{
    //    EventSystem.current.SetSelectedGameObject(null);
    //    yield return new WaitForEndOfFrame();
    //    EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    //}

    //public void MakeChoice(int choiceIndex, string npcId)
    //{
    //    currentStory.ChooseChoiceIndex(choiceIndex);
    //    Debug.Log(npcId);
    //    if (choiceIndex == 0 && npcId.Contains("Quest") && start)
    //    {
    //        //NPC id �� Quest ID�� ��ġ���Ѿ���
    //        Managers.Questevent.StartQuest(npcId);
    //    }
    //    else if (choiceIndex == 0 && npcId.Contains("Quest") && end)
    //    {
    //        Managers.Questevent.FinishQuest(npcId);
    //    }
    //}

}
