using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] GameObject heldObjectContainer;

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
            Debug.Log($"Throw: {heldObject.name}");
            Destroy(heldObject);
            myAnimator.SetBool("isCarrying", false);
        }
        else if (interactableObject)
        {
            Debug.Log($"Interaction: {interactableObject.name}");
            InteractTrigger interaction = interactableObject.GetComponent<InteractTrigger>();
            if (interaction)
            {
                interaction.DoInteraction();
            }
        }
    }

    public void PickupObject(GameObject pickup)
    {
        heldObject = pickup;

        // move object into a child container of the player
        pickup.transform.SetParent(heldObjectContainer.transform);
        pickup.transform.localPosition = new Vector2(0f, 0f);

        // disable collider and interaction trigger
        pickup.GetComponent<Collider2D>().enabled = false;
        pickup.GetComponentInChildren<InteractTrigger>().enabled = false;

        // play animations for pickup and carry
        myAnimator.SetTrigger("Pickup");
        myAnimator.SetBool("isCarrying", true);
    }
}
