using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialoguePopup : UI_Popup
{
    [SerializeField] private GameObject DialoguePopup;
    [SerializeField] public GameObject dialoguePanel { get; private set; }
    [SerializeField] public TextMeshProUGUI dialogueText { get; private set; }
    [SerializeField] public TextMeshProUGUI displayNameText { get; private set; }
    [SerializeField] public Animator portraitAnimator { get; private set; }

    [SerializeField] public TextMeshProUGUI[] choicesText { get; private set; }
    [SerializeField] public GameObject[] choices { get; private set; }
    [SerializeField] public Button[] choiceButton { get; private set; }
    [SerializeField] public DialogueManager DialogueManager { get; private set; }

    enum UIs
    {
        DialoguePanel,
        DialogueChoices
    }

    enum Buttons
    {
        Choice0, 
        Choice1, 
        Choice2
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        DialoguePopup = this.gameObject;
        dialoguePanel = DialoguePopup.transform.GetChild(0).gameObject;
        dialogueText=dialoguePanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        displayNameText=dialoguePanel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();
        portraitAnimator = dialoguePanel.transform.GetChild(3).GetChild(0).GetComponent<Animator>();

        choices = new GameObject[3] { dialoguePanel.transform.GetChild(2).GetChild(0).gameObject, dialoguePanel.transform.GetChild(2).GetChild(1).gameObject,dialoguePanel.transform.GetChild(2).GetChild(2).gameObject };
        choicesText= new TextMeshProUGUI[3] { choices[0].GetComponent<TextMeshProUGUI>(), choices[1].GetComponent<TextMeshProUGUI>(), choices[2].GetComponent<TextMeshProUGUI>() };
        choiceButton = new Button[3] { choices[0].GetComponent<Button>(), choices[1].GetComponent<Button>(), choices[2].GetComponent<Button>() };
        DialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        return true;
    }

}
