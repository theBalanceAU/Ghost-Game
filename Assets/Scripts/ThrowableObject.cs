using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class ThrowableObject : MonoBehaviour
{
    CircleCollider2D myCollider;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Interaction interaction;

    [SerializeField] AudioClip hitSound;

    GameObject holder;

    bool isThrown;
    bool isDestroyed;

    void Awake()
    {
        myCollider = GetComponent<CircleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        interaction = GetComponent<Interaction>();
        if (!interaction)
        {
            interaction = GetComponentInChildren<Interaction>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool hitDetected = false;
        if (isThrown)
        {
            if (other.transform.CompareTag("Wall"))
            {
                Debug.Log($"{name} hit a wall!");
                hitDetected = true;
            }
            else if (other.transform.CompareTag("Enemy"))
            {
                Debug.Log($"{name} hit an enamy ({other.transform.name})!");
                hitDetected = true;
            }
            else if (other.transform.CompareTag("Pickup"))
            {
                Debug.Log($"{name} hit another pickup ({other.transform.name})!");
                hitDetected = true;
            }

            if (hitDetected)
            {
                StartCoroutine(BreakMe());
            }
        }
    }

    IEnumerator BreakMe()
    {
        isDestroyed = true;
        myCollider.enabled = false;
        myRigidBody.velocity = Vector2.zero;
        myAnimator.SetTrigger("Break");

        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
        
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    public void BindToPlayer(GameObject owner, Transform parent)
    {
        Debug.Log($"Binding to player: {name}");
        holder = owner;

        // move object into a child container of the player
        transform.SetParent(parent);
        transform.localPosition = new Vector2(0f, 0f);

        // disable collider and interaction object while being carried
        myCollider.enabled = false;
        interaction.AllowInteraction(false);

        // change object's animation state to picked up
        myAnimator.SetTrigger("Pickup");
    }

    // public void DropObject()
    // {
    //     if (!holder)
    //         return;

    //     Debug.Log($"Dropping: {name}");

    //     transform.SetParent(null);

    //     myCollider.enabled = true;
    //     interaction.AllowInteraction(true);

    //     holder = null;

    //     // if using this later - make sure to set animation state back to idle as the appearance is slightly different
    // }

    public void ThrowObject(Vector2 direction, float throwSpeed, float throwDuration)
    {
        Debug.Log($"ThrowableObject Throwing: {name}");

        isThrown = true;
        holder = null;

        // remove from holding slot on player
        transform.SetParent(null);

        // enable the collider again
        myCollider.enabled = true;

        // make it a trigger so it doesn't accidentally knock anything around
        myCollider.isTrigger = true;

        // yeet it
        StartCoroutine(Yeet(direction, throwSpeed, throwDuration));
    }

    IEnumerator Yeet(Vector2 direction, float throwSpeed, float throwDuration)
    {
        Vector2 startPoint = transform.position;
        Vector2 velocity = direction * throwSpeed;

        myRigidBody.velocity = velocity;

        yield return new WaitForSeconds(throwDuration);
        
        // if thrown to max distance and it's still intact, break it
        if (!isDestroyed)
        {
            Debug.Log($"Max distance reached. Breaking {name}.");
            yield return BreakMe();
        }
    }
}
