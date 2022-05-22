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

    Vector2 moveInput;
    bool isAlive;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        isAlive = true;
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, moveInput.y * runSpeed);
        myRigidBody.velocity = playerVelocity;
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
            return;

        moveInput = isAlive ? value.Get<Vector2>() : new Vector2(0, 0);
    }

    
}
