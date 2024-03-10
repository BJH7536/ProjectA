using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Serialization;
using VInspector;

public class Player : MonoBehaviour
{
    [Tab("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [Tab("Information")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector2 InputVector = new Vector2(0,0);
    [SerializeField] private float jumpPower = 30f;
    [SerializeField] private float velocityLimit = 15.0f;
    private PlayerInputActions _playerInputActions;
    [Tab("NPC Interaction")]
    [SerializeField] public GameObject NPC;
    [SerializeField] private bool isNPCAvailable = false;
    [SerializeField] private bool isAction;
    [SerializeField] private UI_DialoguePopup popup;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] public QuestManager questManager;
    [SerializeField] private int QuestIndex;
    private bool interactPressed = false;
    public int talkIndex;

    private static Player instance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        _playerInputActions = new PlayerInputActions();

        instance = this;
        NPC = GameObject.Find("NULLNPC");
        QuestIndex = 10;
    }



    public static Player GetInstance()
    {
        return instance;
    }

    private void OnChange(InputAction.CallbackContext context)
    {
        if (context.action.actionMap["WeaponExchange"].activeControl.name == "1")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (context.action.actionMap["WeaponExchange"].activeControl.name == "2")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        #region About PlayerInput
        // PlayerInput을 컴포넌트 대신 스크립트로
        _playerInputActions.PlayerAction.Move.started += MoveStarted;
        _playerInputActions.PlayerAction.Move.performed += MovePerformed;
        _playerInputActions.PlayerAction.Move.canceled += MoveCanCeled;
        _playerInputActions.PlayerAction.Jump.started += JumpStarted;
        _playerInputActions.PlayerAction.Jump.performed += JumpPerformed;
        _playerInputActions.PlayerAction.Jump.canceled += JumpCanceled;
        _playerInputActions.PlayerAction.Interact.performed += InteractPerformed;
        _playerInputActions.PlayerAction.WeaponExchange.performed += OnChange;
        _playerInputActions.PlayerAction.Escape.started += PauseOrResume;
        _playerInputActions.Enable();
        #endregion
    }
    
    private void OnDisable()
    {
        #region About PlayerInput
        // PlayerInput을 컴포넌트 대신 스크립트로
        _playerInputActions.PlayerAction.Move.started -= MoveStarted;
        _playerInputActions.PlayerAction.Move.performed -= MovePerformed;
        _playerInputActions.PlayerAction.Move.canceled -= MoveCanCeled;
        _playerInputActions.PlayerAction.Jump.started -= JumpStarted;
        _playerInputActions.PlayerAction.Jump.performed -= JumpPerformed;
        _playerInputActions.PlayerAction.Jump.canceled -= JumpCanceled;
        _playerInputActions.PlayerAction.Interact.performed -= InteractPerformed;
        _playerInputActions.PlayerAction.WeaponExchange.performed -= OnChange;
        _playerInputActions.PlayerAction.Escape.started -= PauseOrResume;
        _playerInputActions.Disable();
        #endregion
    }

    private void FixedUpdate()
    {
        //InputVector = _playerInputActions.PlayerAction.Move.ReadValue<Vector2>();
        if(rb.velocity.magnitude < velocityLimit)
            rb.AddForce(InputVector * speed, ForceMode2D.Impulse);

        Debug.DrawRay(rb.position,Vector3.right,Color.red );
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector3.right, 1, LayerMask.GetMask("Object"));
        if(hit.collider != null)
        {
            isNPCAvailable = true;
            //Debug.Log("NPC Available : " + isNPCAvailable);
            NPC = hit.collider.gameObject;
        }
        if (isNPCAvailable && Vector2.Distance(transform.position, NPC.transform.position) > 3)
        {
            isNPCAvailable = false;
            //Debug.Log("NPC Available : " + isNPCAvailable);
            NPC = GameObject.Find("NULLNPC");
        }

    }
    
    #region Move

    void MoveStarted(InputAction.CallbackContext context)
    {
        //Debug.Log($"MoveStarted {context}");
    }
    void MovePerformed(InputAction.CallbackContext context)
    {
        //Debug.Log($"MovePerformed {context}");
        InputVector = isAction ? Vector2.zero : context.ReadValue<Vector2>();
        
        if (InputVector.x == 0) sr.flipX = sr.flipX;
        else if (InputVector.x < 0) sr.flipX = true;
        else sr.flipX = false;
    }
    void MoveCanCeled(InputAction.CallbackContext context)
    {
        //Debug.Log($"MoveCanceled {context}");
        InputVector = Vector2.zero;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    
    #endregion

    #region Jump
    
    void JumpStarted(InputAction.CallbackContext context)
    {
        //Debug.Log($"JumpStarted {context}");
        rb.AddForce(isAction ? Vector2.zero : Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    void JumpPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log($"JumpPerformed {context}");
    }
    void JumpCanceled(InputAction.CallbackContext context)
    {
        //Debug.Log($"JumpCanceled {context}");
    }

    #endregion

    #region Interact
    void InteractPerformed(InputAction.CallbackContext context)
    {
        Action(NPC);
    }

    public void Action(GameObject scanfobj)
    {
       
        NpcData npcdata= NPC.GetComponent<NpcData>();
        Talk(npcdata.npcId, npcdata.isNpc);
        popup.dialoguePanel.SetActive(isAction);
    }

    void Talk(int id,bool isNpc)
    {
        int questTalkIndex=questManager.GetQuestTalkIndex(id);
        

        string talkData= dialogueManager.GetTalk(id+ questTalkIndex, talkIndex);        //퀘스트번호+NPCId => 퀘스트용 대화 데이터 Id
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            questManager.AdvanceQuest(NPC.GetComponent<NpcData>().questId[0]);
            //questManager.CheckRequirement(QuestIndex);
            return;
        }

        if (isNpc)
        {
            popup.dialogueText.text = talkData.Split(':')[0];
            popup.dialoguePanel.transform.GetChild(3).gameObject.SetActive(true);
            popup.portraitImage.sprite=NPC.GetComponent<NpcData>().npcPortrait[dialogueManager.GetPortraitIndex(id, int.Parse(talkData.Split(':')[1]))];
            
            //popup.portraitAnimator.Play(dialogueManager.GetPortraitIndex(id, int.Parse(talkData.Split(':')[1].Trim())));

            popup.displayNameText.text = NPC.name;
        }
        else
        {
            popup.dialogueText.text = talkData;
            popup.dialoguePanel.transform.GetChild(3).gameObject.SetActive(false);
            popup.displayNameText.text = NPC.name;
        }

        isAction = true;
        talkIndex++;
       
    }

    #endregion

    #region Pause

    void PauseOrResume(InputAction.CallbackContext context)
    {
        if (Managers.UI.FindPopup<UI_PausePopup>() == null)
        {
            Managers.UI.ShowPopupUI<UI_PausePopup>();
        }
        else
        {
            Managers.UI.CloseAllPopupUI();
            Time.timeScale = 1.0f;
        }
        
    }
    #endregion

    #region Quest
    public event Action onQuestLogTogglePressed;
    public void QuestLogTogglePressed()
    {
        if (onQuestLogTogglePressed != null)
        {
            onQuestLogTogglePressed();
        }
    }

    #endregion
}
