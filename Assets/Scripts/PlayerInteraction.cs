using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] GameObject heldObjectContainer;
    [SerializeField] float throwSpeed = 10f;
    [SerializeField] float throwDuration = 0.5f;

    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    Animator myAnimator;

    GameObject interactableObject;
    GameObject heldObject;

    // Unity Events

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interaction"))
        {
            interactableObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interaction") && interactableObject == other.gameObject)
        {
            // Clear the interactable object that we have moved away from
            interactableObject = null;
        }
    }

    // Player Input events

    void OnInteract(InputValue value)
    {
        if (heldObject)
        {
            ThrowHeldObject();
        }
        else if (interactableObject)
        {
            Debug.Log($"Interaction: {interactableObject.name}");
            Interaction interaction = interactableObject.GetComponent<Interaction>();
            if (interaction)
            {
                interaction.DoInteraction(this);
            }
        }
    }

    public void PickupObject(GameObject item)
    {
        heldObject = item;

        // move object into a child container of the player
        item.transform.SetParent(heldObjectContainer.transform);
        item.transform.localPosition = new Vector2(0f, 0f);

        // disable collider and interaction trigger
        item.GetComponent<Collider2D>().enabled = false;
        item.GetComponentInChildren<Interaction>().gameObject.SetActive(false);

        // play animations for pickup and carry
        myAnimator.SetTrigger("Pickup");
        myAnimator.SetBool("isCarrying", true);
    }

    void DropHeldObject()
    {
        if (!heldObject)
            return;

        Debug.Log($"Dropping: {heldObject.name}");

        heldObject.transform.SetParent(null);

        Vector2 startPoint = heldObject.transform.position;
        Vector2 endPoint = new Vector2(heldObject.transform.position.x + 2f, heldObject.transform.position.y);

        heldObject.GetComponent<Collider2D>().enabled = true;
        heldObject.GetComponentInChildren<Interaction>().gameObject.SetActive(true);
        heldObject = null;

        myAnimator.SetBool("isCarrying", false);
    }

    void ThrowHeldObject()
    {
        if (!heldObject)
            return;

        Debug.Log($"Throwing: {heldObject.name}");

        // remove from holding slot on player
        heldObject.transform.SetParent(null);

        // enable the collider again
        heldObject.GetComponent<Collider2D>().enabled = true;

        // yeet it
        StartCoroutine(Throw(heldObject));

        // clear the reference so we are no longer holding this object
        heldObject = null;

        // change player animation state
        myAnimator.SetBool("isCarrying", false);
    }

    IEnumerator Throw(GameObject item)
    {
        Vector2 startPoint = item.transform.position;
        Vector2 velocity = Vector2.right * throwSpeed;

        item.GetComponent<Rigidbody2D>().velocity = velocity;

        yield return new WaitForSeconds(throwDuration);
        
        // for now, just stop the object and enable interaction again
        item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        item.GetComponentInChildren<Interaction>().gameObject.SetActive(true);
    }
}
