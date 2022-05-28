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

    public Vector2 facing;

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
        facing = Vector2.down;

        if (GameManager.Instance.IsPlayerSpawnOverride())
            transform.position = GameManager.Instance.GetPlayerSpawn();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    // Player Input events

    void OnMove(InputValue value)
    {
        moveInput = isAlive ? value.Get<Vector2>() : Vector2.zero;
    }

    void OnPause(InputValue value)
    {
        Time.timeScale = (Time.timeScale == 0f) ? 1f : 0f;

        // TODO - pause the game, show pause scene
        // GameManager.Instance.PauseGame();
        
        // -> SceneManager.LoadSceneAsync("PauseScreen", LoadSceneMode.Additive);
        Debug.Log($"Pause");
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

        if (isMovingHorizontal)
        {
            facing = playerVelocity.x > 0 ? Vector2.right : Vector2.left;
            myAnimator.SetFloat("xFacing", facing.x);
            myAnimator.SetFloat("yFacing", facing.y);
        }
        else if (isMovingVertical)
        {
            facing = playerVelocity.y > 0 ? Vector2.up : Vector2.down;
            myAnimator.SetFloat("xFacing", facing.x);
            myAnimator.SetFloat("yFacing", facing.y);
        }
    }

    
}
