using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] bool canScare;
    [SerializeField] float disappearAfterSeconds = 2f;
    bool isScared;

    Rigidbody2D myRigidBody;
    Animator myAnimator;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isScared)
        {
            // TODO
        }
    }

    IEnumerator RunAndDisappear()
    {
        Vector2 playerVelocity = new Vector2(1f, 1f);

        myRigidBody.velocity = playerVelocity;

        bool isMovingHorizontal = (playerVelocity.x > Mathf.Epsilon) || (playerVelocity.x < -Mathf.Epsilon);
        bool isMovingVertical = (playerVelocity.y > Mathf.Epsilon) || (playerVelocity.y < -Mathf.Epsilon);

        myAnimator?.SetBool("isWalking", isMovingHorizontal || isMovingVertical);
        myAnimator?.SetFloat("xVelocity", playerVelocity.x);
        myAnimator?.SetFloat("yVelocity", playerVelocity.y);

        yield return new WaitForSeconds(disappearAfterSeconds);

        Destroy(gameObject);
    }

    public void Scare(Transform source)
    {
        if (canScare)
        {
            Debug.Log($"{name} is scared");
            isScared = true;
            StartCoroutine(RunAndDisappear());
        }
    }
}
