using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] GameObject heldObjectContainer;
    [SerializeField] float throwSpeed = 10f;
    [SerializeField] float throwDuration = 0.5f;

    PlayerMovement playerMovement;
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    Animator myAnimator;

    GameObject interactableObject;
    ThrowableObject throwableObject;

    // Unity Events

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
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
        if (throwableObject)
        {
            ThrowHeldObject();
        }
        else if (interactableObject)
        {
            Debug.Log($"Interaction: {interactableObject.name}");
            Interaction interaction = interactableObject.GetComponent<Interaction>();
            if (interaction)
            {
                interaction.DoInteraction();
            }
        }
    }

    public void PickupObject(GameObject item)
    {
        Debug.Log($"Pick up: {item.name}");
        throwableObject = item.GetComponent<ThrowableObject>();

        if (!throwableObject)
        {
            Debug.Log($"Could not pick up object {item.name}");
            return;
        }

        throwableObject.BindToPlayer(gameObject, heldObjectContainer.transform);

        // trigger animations on player for pickup and carry
        myAnimator.SetTrigger("Pickup");
        myAnimator.SetBool("isCarrying", true);
    }

    // void DropHeldObject()
    // {
    //     throwableObject?.DropObject();
    //     throwableObject = null;
    //     myAnimator.SetBool("isCarrying", false);
    // }

    void ThrowHeldObject()
    {
        if (!throwableObject)
            return;

        throwableObject.ThrowObject(playerMovement.facing, throwSpeed, throwDuration);

        // clear the local reference so we are no longer holding this object
        throwableObject = null;

        // change player animation state
        myAnimator.SetBool("isCarrying", false);
    }
}
