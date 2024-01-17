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
    private PlayerInputActions _playerInputActions;

    [Header("NPC Interaction")]
    [SerializeField] public GameObject NPC;
    [SerializeField] private bool isNPCAvailable = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

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
        #endregion
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

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
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

        Debug.DrawRay(rb.position,Vector3.right,Color.red );
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector3.right, 1, LayerMask.GetMask("NPC"));
        if(hit.collider != null)
        {
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
}
