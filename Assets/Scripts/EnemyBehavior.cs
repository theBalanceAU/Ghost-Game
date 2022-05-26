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
        Vector2 velocity = new Vector2(1f, 1f);

        myRigidBody.velocity = velocity;

        bool isMovingHorizontal = (velocity.x > Mathf.Epsilon) || (velocity.x < -Mathf.Epsilon);
        bool isMovingVertical = (velocity.y > Mathf.Epsilon) || (velocity.y < -Mathf.Epsilon);

        myAnimator?.SetBool("isWalking", isMovingHorizontal || isMovingVertical);
        myAnimator?.SetFloat("xVelocity", velocity.x);
        myAnimator?.SetFloat("yVelocity", velocity.y);

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
