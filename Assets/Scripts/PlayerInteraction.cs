using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    Animator myAnimator;

    // public for testing only - change to private later
    public GameObject interaction;

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
            interaction = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interaction") && interaction == other.gameObject)
        {
            // Clear the interactable object that we have moved away from
            interaction = null;
        }
    }

    // Player Input events

    void OnInteract(InputValue value)
    {
        Debug.Log("Interact");

        if (!interaction)
            return;

        Debug.Log($"{interaction.name}");
        InteractTrigger trigger = interaction.GetComponent<InteractTrigger>();
        if (trigger)
        {
            trigger.DoInteraction();
        }
    }
}
