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
    GameObject interactTrigger;

    GameObject holder;

    bool isThrown;

    void Awake()
    {
        myCollider = GetComponent<CircleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        
    }

    private void Start()
    {
        interactTrigger = FindObjectOfType<Interaction>()?.gameObject;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isThrown)
        {
            if (other.transform.CompareTag("Wall"))
            {
                Debug.Log($"{name} hit a wall!");
            }
            else if (other.transform.CompareTag("Enemy"))
            {
                Debug.Log($"{name} hit an enamy ({other.transform.name})!");
            }
            else if (other.transform.CompareTag("Pickup"))
            {
                Debug.Log($"{name} hit another pickup ({other.transform.name})!");
            }

            myRigidBody.velocity = Vector2.zero;
            myAnimator.SetTrigger("Break");
            //Destroy(gameObject);
        }
    }

    public void PickupObject(GameObject owner, Transform parent)
    {
        holder = owner;

        // move object into a child container of the player
        transform.SetParent(parent);
        transform.localPosition = new Vector2(0f, 0f);

        // disable collider and interaction trigger
        myCollider.enabled = false;
        interactTrigger.SetActive(false);

        // change animation state to picked up
        myAnimator.SetTrigger("Pickup");
    }

    public void DropObject()
    {
        if (!holder)
            return;

        Debug.Log($"Dropping: {name}");

        transform.SetParent(null);

        myCollider.enabled = true;
        interactTrigger.SetActive(true);

        holder = null;

        // if using this later - make sure to set animation state back to idle as the appearance is slightly different
    }

    public void ThrowObject(float throwSpeed, float throwDuration)
    {
        Debug.Log($"ThrowableObject Throwing: {name}");

        isThrown = true;
        holder = null;

        // remove from holding slot on player
        transform.SetParent(null);

        // enable the collider again
        myCollider.enabled = true;

        // yeet it
        StartCoroutine(Yeet(throwSpeed, throwDuration));
    }

    IEnumerator Yeet(float throwSpeed, float throwDuration)
    {
        Vector2 startPoint = transform.position;
        Vector2 velocity = Vector2.right * throwSpeed;

        myRigidBody.velocity = velocity;

        yield return new WaitForSeconds(throwDuration);
        
        // for now, just stop the object and enable interaction again
        myRigidBody.velocity = Vector2.zero;
        interactTrigger.SetActive(true);

        Destroy(gameObject);
    }
}
