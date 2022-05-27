using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] bool canScare;
    [SerializeField] float disappearAfterSeconds = 2f;
    
    float runSpeed = 5f;

    bool isScared;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isScared)
        {
            // TODO
        }
    }

    IEnumerator RunAndDisappear(Transform source)
    {
        // get opposite direction from attack
        Vector2 oppositeDirection = transform.position - source.transform.position;

        // normalize the direction (removes magnitude so that the distance between the two points doesn't matter)
        oppositeDirection.Normalize();

        // apply speed
        Vector2 velocity = oppositeDirection * runSpeed;

        // turn collider into a trigger to avoid it bumping into things
        myCollider.isTrigger = true;

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
            StartCoroutine(RunAndDisappear(source));
        }
    }
}
