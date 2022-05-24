using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class InteractTrigger : MonoBehaviour
{
    [Header("Load Scene")]
    [SerializeField] bool loadScene;
    [SerializeField] string sceneName;
    [SerializeField] float delaySceneLoad;

    [Header("Play Sound")]
    [SerializeField] bool playSound;
    [SerializeField] AudioClip soundClip;

    [Header("Pickup Object")]
    [SerializeField] bool pickupObject;
    [SerializeField] GameObject sourceObject;
    [SerializeField] bool destroyObjectOnPickup;
    
    PlayerInteraction playerInteraction;

    void Awake()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // show prompt for interact button?
            Debug.Log("Press [interact] button");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // hide prompt
            Debug.Log("Player out of trigger zone");
        }
    }

    public void DoInteraction()
    {
        Debug.Log($"DoInteraction()");

        if (loadScene && !string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"LOAD SCENE {sceneName}");
            if (delaySceneLoad > 0)
            {
                Debug.Log($"Delay for {delaySceneLoad} seconds");
            }
            else
            {
                //SceneManager.LoadSceneAsync(sceneName);
            }
            
        }

        if (playSound && soundClip)
        {
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
        }

        if (pickupObject && sourceObject)
        {
            Debug.Log($"Pickup object {sourceObject.name}");
            playerInteraction?.PickupObject(sourceObject);
        }
    }
}
