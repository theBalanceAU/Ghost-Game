using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float runSpeed = 1f;

    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    Animator myAnimator;

    Vector2 moveInput;
    bool isAlive;

    // Unity Events

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        isAlive = true;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    // Player Input events

    void OnMove(InputValue value)
    {
        if (!isAlive)
            return;

        moveInput = isAlive ? value.Get<Vector2>() : new Vector2(0, 0);
    }

    // Private methods

    void MovePlayer()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, moveInput.y * runSpeed);
        myRigidBody.velocity = playerVelocity;

        bool isMovingHorizontal = (playerVelocity.x > Mathf.Epsilon) || (playerVelocity.x < -Mathf.Epsilon);
        bool isMovingVertical = (playerVelocity.y > Mathf.Epsilon) || (playerVelocity.y < -Mathf.Epsilon);

        // Debug.Log($"x={playerVelocity.x}, y={playerVelocity.y}");
        // Debug.Log($"isMovingHorizontal={isMovingHorizontal}, isMovingVertical={isMovingVertical}");

        myAnimator.SetBool("isWalking", isMovingHorizontal || isMovingVertical);
        myAnimator.SetFloat("xVelocity", playerVelocity.x);
        myAnimator.SetFloat("yVelocity", playerVelocity.y);
    }

    
}
