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
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private Button[] choiceButton;
    
    private Story currentStory;
    private int num = 3;

    public bool dialogueIsPlaying { get; private set; }
    
    public static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    //public void Set()           //버튼 및 패널 붙이기
    //{
        
    //}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager");
        }
        instance = this;
        dialoguePanel = GameObject.Find("Canvas/DialoguePanel");
        dialogueText = GameObject.Find("Canvas/DialoguePanel/DialogueText").transform.GetComponent<TextMeshProUGUI>();
        displayNameText = GameObject.Find("Canvas/DialoguePanel/SpeakerFrame/DisplayNameText").transform.GetComponent<TextMeshProUGUI>();
        portraitAnimator = GameObject.Find("Canvas/DialoguePanel/PortraitFrame/PortraitImage").transform.GetComponent<Animator>();
        choices = new GameObject[3] { GameObject.Find("Canvas/DialoguePanel/DialogueChoices/Choice0"), GameObject.Find("Canvas/DialoguePanel/DialogueChoices/Choice1"), GameObject.Find("Canvas/DialoguePanel/DialogueChoices/Choice2") };
        choicesText = new TextMeshProUGUI[3];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = GameObject.Find("Canvas/DialoguePanel/DialogueChoices/Choice" + i + "/Text (TMP)").transform.GetComponent<TextMeshProUGUI>();
        }
        choiceButton = new Button[3] { GameObject.Find("Canvas/DialoguePanel/DialogueChoices/Choice0").transform.GetComponent<Button>(), GameObject.Find("Canvas/DialoguePanel/DialogueChoices/Choice1").transform.GetComponent<Button>(), GameObject.Find("Canvas/DialoguePanel/DialogueChoices/Choice2").transform.GetComponent<Button>() };
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
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

        if(currentChoices.Count > choices.Length)
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

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
} 
