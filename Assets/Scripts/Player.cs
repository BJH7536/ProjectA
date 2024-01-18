using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector2 InputVector = new Vector2(0,0);
    [SerializeField] private float jumpPower = 30f;
    [SerializeField] private float velocityLimit = 15.0f;
    [SerializeField] public float playerHP;
    [SerializeField] private float playerDamage;
    [SerializeField] private Tool tool;
    private PlayerInputActions _playerInputActions;

    [Header("NPC Interaction")]
    [SerializeField] public GameObject NPC;
    [SerializeField] private bool isNPCAvailable = false;
    [SerializeField] private RaycastHit2D target;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        tool=GetComponentInChildren<Tool>();

        #region About PlayerInput
        
        // PlayerInput을 컴포넌트 대신 스크립트로
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.PlayerAction.Move.started += MoveStarted;
        _playerInputActions.PlayerAction.Move.performed += MovePerformed;
        _playerInputActions.PlayerAction.Move.canceled += MoveCanCeled;
        _playerInputActions.PlayerAction.Jump.started += JumpStarted;
        _playerInputActions.PlayerAction.Jump.performed += JumpPerformed;
        _playerInputActions.PlayerAction.Jump.canceled += JumpCanceled;
        _playerInputActions.PlayerAction.Interact.started += InteractStarted;
        _playerInputActions.PlayerAction.Interact.performed += InteractPerformed;
        _playerInputActions.PlayerAction.Interact.canceled += InteractCanceled;
        _playerInputActions.PlayerAction.WeaponExchange.performed += OnChange;
        _playerInputActions.PlayerAction.Attack.performed += OnAttack;
        #endregion
    }




    private void Start()
    {
        Managers.Sound.playSoundEffect("str");
    }

    private void FixedUpdate()
    {
        InputVector = _playerInputActions.PlayerAction.Move.ReadValue<Vector2>();
        if(rb.velocity.magnitude < velocityLimit)
            rb.AddForce(InputVector * speed, ForceMode2D.Impulse);


        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector3.right, 1, LayerMask.GetMask("NPC"));
        if(hit.collider != null)
        {
            target = hit;
            isNPCAvailable = true;
            Debug.Log("NPC Available : " + isNPCAvailable);
            NPC = GameObject.Find("NPC");     
        }
        if (isNPCAvailable && Vector2.Distance(transform.position, NPC.transform.position) > 3)
        {
            isNPCAvailable = false;
            Debug.Log("NPC Available : " + isNPCAvailable);
            NPC = GameObject.Find("NULLNPC");
        }

    }
    
    #region Move

    void MoveStarted(InputAction.CallbackContext context)
    {
        Debug.Log($"MoveStarted {context}");
    }
    void MovePerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"MovePerformed {context}");
        InputVector = context.ReadValue<Vector2>();
        
        if (InputVector.x == 0) sr.flipX = sr.flipX;
        else if (InputVector.x < 0) sr.flipX = true;
        else sr.flipX = false;
    }
    void MoveCanCeled(InputAction.CallbackContext context)
    {
        Debug.Log($"MoveCanceled {context}");
        InputVector = Vector2.zero;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    
    #endregion

    #region Jump
    
    void JumpStarted(InputAction.CallbackContext context)
    {
        Debug.Log($"JumpStarted {context}");
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    void JumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"JumpPerformed {context}");
    }
    void JumpCanceled(InputAction.CallbackContext context)
    {
        Debug.Log($"JumpCanceled {context}");
    }

    #endregion

    #region Interact
    void InteractStarted(InputAction.CallbackContext context)
    {
        Debug.Log($"InteractStarted {context}");
    }
    void InteractPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"InteractPerformed {context}");
    }
    void InteractCanceled(InputAction.CallbackContext context)
    {
        Debug.Log($"InteractCanceled {context}");
    }
    #endregion

    #region WeaponChange
    private void OnChange(InputAction.CallbackContext context)
    {
        //if (context.action.actionMap["WeaponExchange"].activeControl.name == "1")
        //{
        //    transform.GetChild(0).gameObject.SetActive(true);
        //    transform.GetChild(1).gameObject.SetActive(false);
        //    tool = transform.GetChild(0).GetComponent<Tool>();
        //}
        //else if ( == "2")
        //{
        //    transform.GetChild(0).gameObject.SetActive(false);
        //    transform.GetChild(1).gameObject.SetActive(true);
        //    tool = transform.GetChild(1).GetComponent<Tool>();
        //}

        Tool[] list= GetComponentsInChildren<Tool>();
        foreach(Tool t in list)
        {
            t.gameObject.SetActive(false);
        }
        int Toolnum = int.Parse(context.action.actionMap["WeaponExchange"].activeControl.name)-1;
        transform.GetChild(Toolnum).gameObject.SetActive(true);
        tool= transform.GetChild(Toolnum).GetComponent<Tool>();
        playerDamage=transform.GetChild(Toolnum).GetComponent<Tool>().getToolDamage();
    }
    #endregion


    #region Attack
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.action.actionMap["Attack"].activeControl.name == "x" && isNPCAvailable)
        {
            AttackorDecomp();
        }

        //Raycast에서 가져온 변수값 필요
    }

    void AttackorDecomp()
    {
        //모션
        switch (target.transform.tag)
        {
            case "Enemy":
                    break;
            case "Parts":
                break;
            default:
                break;
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //GameManager에서 Enemy damage를 가져와서 hp에 빼준다.
            playerHP -= collision.GetComponent<Enemy>().enemyDamage;
           //IEnumerator knockBack()
           //{

            //    //yield return null;  // 1프레임 쉬기
            //    //yield return new WaitForSeconds(2f);    // 2초 쉬기
            //    yield return wait;//하나의 물리 프레임을 딜레이 주기
            //    Vector3 playerPos = GameManager.instance.player.transform.position;
            //    Vector3 dirVec = transform.position - playerPos;
            //    rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

            //}
        }
    }



}
