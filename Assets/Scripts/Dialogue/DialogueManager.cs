using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;              //밑바탕
    [SerializeField] private TextMeshProUGUI dialogueText;          //대화창
    [SerializeField] private TextMeshProUGUI displayNameText;       //NPC이름
    [SerializeField] private Animator portraitAnimator;             //NPC초상화
    private Animator layoutAnimator;                                //레이아웃

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;                  //선택지버튼
    private TextMeshProUGUI[] choicesText;                          //선택지 텍스트
    private Button[] choiceButton;                                  //선택지 버튼
    
    private Story currentStory;                                     //Ink 로 생성된 텍스트를 받아올 Class변수
   

    public bool dialogueIsPlaying { get; private set; }             //현재 대화창에 진입했는지 확인할 변수
    
    public static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";                   //테그값들 테그값 : 변수
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private UI_DialoguePopup popup;

    public string npcId;
    [Header("Qeust state")]
    [SerializeField] public bool start;
    [SerializeField] public bool end;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager");
        }
        popup = GameObject.Find("UI_DialoguePopup").GetComponent<UI_DialoguePopup>();
        instance= this;
        start = true; 
        end=false;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
       
        dialoguePanel = popup.dialoguePanel;
        dialogueText = popup.dialogueText;
        displayNameText = popup.displayNameText;
        portraitAnimator = popup.portraitAnimator;
        choices = popup.choices;
        choiceButton = popup.choiceButton;
        choicesText = popup.choicesText;
        
        for (int i = 0; i < choices.Length; i++)
        {
            int id = i;
            choiceButton[i].onClick.AddListener(() => MakeChoice(id,npcId));
        }
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator=dialoguePanel.GetComponent<Animator>();

        choicesText =new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index]=choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if(Player.GetInstance().GetInteractPressed())
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //태그 초기화
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)                   //더 보여줄 이야기가 있다면
        {
            dialogueText.text = currentStory.Continue();            //한줄 출력
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
            string[] splitTag=tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag parsed error : " + tag);
            }
            string tagkey = splitTag[0].Trim();
            string tagvalue = splitTag[1].Trim();

            switch(tagkey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagvalue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagvalue);
                    break;
                case LAYOUT_TAG: 
                    layoutAnimator.Play(tagvalue);
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

        if(currentChoices.Count > choices.Length)           //현재 선택지의 개수가 버튼의 개수보다 많으면 오류 
        {
            Debug.LogError("More choices than ever");
        }

        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index;i< choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex,string npcId)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        Debug.Log(npcId);
        if (choiceIndex == 0 && npcId.Contains("Quest")&&start)
        {
            //NPC id 와 Quest ID를 일치시켜야함
            Managers.Questevent.StartQuest(npcId);
        }else if(choiceIndex == 0 && npcId.Contains("Quest") && end)
        {
            Managers.Questevent.FinishQuest(npcId);
        }
    }
} 
