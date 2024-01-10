using System;
using System.Collections;
using System.Collections.Generic;
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
}
