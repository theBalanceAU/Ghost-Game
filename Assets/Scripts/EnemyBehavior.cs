using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Scare Behavior")]
    [SerializeField] bool canScare;
    [SerializeField] float disappearAfterSeconds = 2f;

    [Header("Enemy Patrol Path")]
    [SerializeField] bool hideInEditor;
    [SerializeField] List<Vector2> waypoints;

    float runSpeed = 2f;
    bool isScared;
    int nextWaypointIndex = 0;
    Vector2 initialRuntimePosition;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();

        initialRuntimePosition = transform.position;
    }

    void Update()
    {
        FollowWaypoints();
    }

    void FollowWaypoints()
    {
        if (isScared || waypoints == null || waypoints.Count == 0)
            return;

        const float tolerance = 0.01f;

        Vector3 nextWaypoint = initialRuntimePosition;
        if (nextWaypointIndex >= 0)
            nextWaypoint = (Vector3)waypoints[nextWaypointIndex];

        
        Vector3 direction = -(transform.position - nextWaypoint);
        direction.Normalize();

        myRigidBody.velocity = direction * runSpeed;

        UpdateAnimator();

        // if we reached destination, point to next waypoint
        Collider2D[] objectsAtWaypoint = Physics2D.OverlapCircleAll(nextWaypoint, tolerance);
        if (objectsAtWaypoint.Length > 0)
        {
            for (int i = 0; i < objectsAtWaypoint.Length; i++)
            {
                if (objectsAtWaypoint[i] == myCollider)
                {
                    nextWaypointIndex++;
                    if (nextWaypointIndex >= waypoints.Count)
                        nextWaypointIndex = -1;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (hideInEditor)
            return;

        const float waypointRadius = 0.2f;
        Vector3 startPosition = transform.position;
        if (initialRuntimePosition != Vector2.zero)
            startPosition = initialRuntimePosition;

        // show waypoints in Unity editor
        if (waypoints != null && waypoints.Count > 0)
        {
            Vector3 previousWaypoint = startPosition + Vector3.zero;
            Gizmos.DrawWireSphere(startPosition, waypointRadius);
            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 nextWaypoint = (Vector3)waypoints[i];
                Gizmos.DrawWireSphere(nextWaypoint, waypointRadius);
                Gizmos.DrawLine(previousWaypoint, nextWaypoint);
                previousWaypoint = nextWaypoint;
            }
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

        UpdateAnimator();

        yield return new WaitForSeconds(disappearAfterSeconds);

        Destroy(gameObject);
    }

    void UpdateAnimator()
    {
        Vector2 velocity = myRigidBody.velocity;

        bool isMovingHorizontal = (velocity.x > Mathf.Epsilon) || (velocity.x < -Mathf.Epsilon);
        bool isMovingVertical = (velocity.y > Mathf.Epsilon) || (velocity.y < -Mathf.Epsilon);

        myAnimator?.SetBool("isWalking", isMovingHorizontal || isMovingVertical);
        myAnimator?.SetFloat("xVelocity", velocity.x);
        myAnimator?.SetFloat("yVelocity", velocity.y);
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
